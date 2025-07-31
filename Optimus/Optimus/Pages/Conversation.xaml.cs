using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using Optimus.AgentEngine;
using Optimus.AgentEngine.Factories;
using Optimus.Commons.Enums;
using Optimus.Helpers;
using Optimus.SemanticKernelPlugins;
using Optimus.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;

namespace Optimus.Pages
{
    public sealed partial class Conversation : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly ObservableCollection<ChatMessageViewModel> Messages = [];
        private readonly ObservableCollection<string> AgentSuggestions = [];

        private readonly Microsoft.UI.Dispatching.DispatcherQueue _dispatcherQueue;
        private AgentChatCompletionService _chatCompletionService;

        private bool _isLoading = false;
        private bool _isImeActive = true;
        private string _globalInstruction = "You are a helpful assistant. Please answer the user's questions concisely and accurately.";

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public Conversation()
        {
            InitializeComponent();
            _dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
        }

        #region Setters
        private void SetLoading(bool isLoading)
        {
            IsLoading = isLoading;
        }

        private void SetUserMessage(string message)
        {
            Messages.Add(ChatMessageViewModel.CreateUserMessage(message));
        }

        private void SetAgentMessage(string message)
        {
            Messages.Add(ChatMessageViewModel.CreateAgentMessage(message));
        }

        private async Task SetAgentSuggestionsAsync(string prompt)
        {
            AgentSuggestions.Clear();

            try
            {
                var sampleAiServiceProvider = AiServiceProvider.Gemini;
                var modelId = "gemini-2.0-flash-lite";
                var kernelFactory = sampleAiServiceProvider switch
                {
                    AiServiceProvider.Gemini => new KernelFactory().UseGoogleGeminiProvider(Cache.ApiKey, modelId),
                    _ => throw new NotSupportedException("The specified AI service provider is not supported.")
                };

                var chatCompletionService = new AgentChatCompletionService(kernelFactory)
                    .WithSystemInstruction(_globalInstruction)
                    .WithChatHistories(_chatCompletionService.ChatHistories);

                var response = await chatCompletionService.SendMessageAsync<List<string>>(prompt);

                foreach (var item in response)
                {
                    AgentSuggestions.Add(item);
                }
            }
            catch
            {
                // Skip the error handling for simplicity
            }
        }
        #endregion

        private async Task ResetConversationAsync()
        {
            Messages.Clear();
            AgentSuggestions.Clear();

            var sampleAiServiceProvider = AiServiceProvider.Gemini;
            var modelId = "gemini-2.5-flash";
            var kernelFactory = sampleAiServiceProvider switch
            {
                AiServiceProvider.Gemini => new KernelFactory().UseGoogleGeminiProvider(Cache.ApiKey, modelId),
                _ => throw new NotSupportedException("The specified AI service provider is not supported.")
            };

            _chatCompletionService = new AgentChatCompletionService(kernelFactory.WithPlugins(Cache.SemanticKernelPlugins))
                .WithSystemInstruction(_globalInstruction);

            var welcomeMessage = await _chatCompletionService.SendMessageAsync(@"To start the conversation, please introduce to me about yourself very *briefly* and *concisely*, such as who you are, what you can do, how you can help me, and some good practices for me to help you to do the task effectively. 
Treat me as your best friend, avoid using a formal-like tone while talking to me; just use a natural, friendly tone with daily-life words when talking to me, like you are talking with your friends in real life.");

            SetAgentMessage(welcomeMessage.ToString());
        }

        private async Task HandleUserInputAsync(string userInput)
        {
            try
            {
                SetLoading(true);
                SetUserMessage(userInput);

                AgentSuggestions.Clear();

                var result = await _chatCompletionService.SendMessageAsync(userInput, 0.7);

                SetAgentMessage(result.ToString());

                await SetAgentSuggestionsAsync("Based on the current context, please provide up to 4 **short** and **concise** suggestions as the follow-up replies in my viewpoint, for me to use as the quick reply in order to continue the task, assuming I am not technical but want to understand and explore the data for analysis purpose or predictional purpose.");
            }
            catch (Exception ex)
            {
                SetAgentMessage($"**Error:**\n\n```console\n{ex.Message}. {ex.InnerException?.Message}\n```\n\n.\n\n*The reason detail has been copied to your clipboard.*");
            }
            finally
            {
                SetLoading(false);
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await ResetConversationAsync();
        }

        private void QueryBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            var textbox = sender as TextBox;

            if (textbox == null)
            {
                return;
            }

            if (e.Key == VirtualKey.Enter
                && !InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Shift).HasFlag(CoreVirtualKeyStates.Down)
                && !string.IsNullOrWhiteSpace(textbox.Text)
                && !_isImeActive)
            {
                var cursorPosition = textbox.SelectionStart;
                var text = textbox.Text;
                if (cursorPosition > 0 && (text[cursorPosition - 1] == '\n' || text[cursorPosition - 1] == '\r'))
                {
                    text = text.Remove(cursorPosition - 1, 1);
                    textbox.Text = text;
                }

                textbox.SelectionStart = cursorPosition - 1;

                var currentPlaceholder = textbox.PlaceholderText;

                textbox.PlaceholderText = "Please wait for the response to complete before entering a new message";
                SendButton_Click(sender, e);
                textbox.PlaceholderText = currentPlaceholder;
            }
            else
            {
                _isImeActive = true;
            }
        }

        private void QueryBox_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            _isImeActive = false;
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            var userInput = QueryBox.Text.Trim();
            QueryBox.Text = string.Empty;

            if (string.IsNullOrEmpty(userInput))
            {
                return;
            }

            await HandleUserInputAsync(userInput);
        }

        private async void ResetConversationButton_Click(object sender, RoutedEventArgs e)
        {
            //if (Messages.Count == 0)
            //{
            //    await DialogHelper.ShowErrorAsync();
            //}

            //var result = await DialogHelper.ShowDialogWithOptions("Reset the conversation", "This action will clear our conversation. Are you sure to proceed?", "Yes");
            //if (result != ContentDialogResult.Primary)
            //{
            //    return;
            //}

            //SetLoading(true);

            //try
            //{
            //    await ResetConversationAsync();
            //}
            //catch (Exception ex)
            //{
            //    ex.CopyToClipboard();
            //    await ShowInforBarAsync($"Error while resetting the conversation: {ex.Message}", false);
            //}

            //SetLoading(false);
        }
    }
}
