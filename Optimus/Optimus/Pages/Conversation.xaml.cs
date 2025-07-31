using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.UI.Xaml.Controls;
using Optimus.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Optimus.Pages
{
    public sealed partial class Conversation : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly ObservableCollection<ChatMessageViewModel> ChatMessages = [];

        public Conversation()
        {
            InitializeComponent();

            ChatMessages = new ObservableCollection<ChatMessageViewModel>
            {
                new ChatMessageViewModel { Role = AuthorRole.User, Message = "Hello, how are you?" },
                new ChatMessageViewModel { Role = AuthorRole.Assistant, Message = "I'm just a program, but I'm here to help!" },
                new ChatMessageViewModel { Role = AuthorRole.User, Message = "What can you do?" },
                new ChatMessageViewModel { Role = AuthorRole.Assistant, Message = "I can assist with various tasks and answer questions." }
            };
        }
    }
}
