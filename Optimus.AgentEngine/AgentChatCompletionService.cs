using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Optimus.AgentEngine.Factories;
using Optimus.Commons.Enums;
using System.Text.Json;

namespace Optimus.AgentEngine
{
    public class AgentChatCompletionService(KernelFactory kernelFactory, int maxMessageCount = 200)
    {
        private readonly IChatCompletionService _chatCompletionService = kernelFactory.Build().GetRequiredService<IChatCompletionService>();
        private readonly int _maxMessageCount = maxMessageCount;

        public readonly Kernel Kernel = kernelFactory.Build();

        public AiServiceProvider ServiceProvider { get; private set; } = kernelFactory.ServiceProvider;
        public ChatHistory ChatHistories { get; private set; } = [];

        public AgentChatCompletionService WithSystemInstruction(string systemInstruction)
        {
            if (string.IsNullOrWhiteSpace(systemInstruction))
            {
                throw new ArgumentException("System instruction cannot be null or empty.", nameof(systemInstruction));
            }

            ChatHistories.Clear();

            ChatHistories.Add(
                new()
                {
                    Role = AuthorRole.System,
                    Content = systemInstruction
                }
            );

            return this;
        }
        public AgentChatCompletionService WithChatHistories(ChatHistory chatHistories)
        {
            if (chatHistories.Count > 0)
            {
                ChatHistories = chatHistories;
            }

            return this;
        }

        public void AddFunctionCallingResponse(FunctionResultContent functionResultContent)
        {
            ChatHistories.Add(functionResultContent.ToChatMessage());
        }

        public async Task<ChatMessageContent> SendMessageAsync(string message, double temperature = 1, int maxOutputToken = int.MaxValue)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Message cannot be null or empty.", nameof(message));
            }

            ChatHistories.AddUserMessage(message.Trim());

            if (ChatHistories.Count > _maxMessageCount)
            {
                var reducer = new ChatHistoryTruncationReducer(targetCount: _maxMessageCount);
                var reducedMessages = await reducer.ReduceAsync(ChatHistories);
                if (reducedMessages is not null)
                {
                    ChatHistories = [.. reducedMessages];
                }
            }

            var response = await _chatCompletionService.GetChatMessageContentAsync(
                ChatHistories,
                executionSettings: ServiceProvider.CreatePromptExecutionSettingsWithFunctionCalling(maxOutputToken, temperature),
                kernel: Kernel);

            ChatHistories.Add(response);

            return response;
        }

        public async Task<T> SendMessageAsync<T>(string message, double temperature = 1, int maxOutputToken = int.MaxValue)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Message cannot be null or empty.", nameof(message));
            }

            ChatHistories.AddUserMessage(message.Trim());

            if (ChatHistories.Count > _maxMessageCount)
            {
                var reducer = new ChatHistoryTruncationReducer(targetCount: _maxMessageCount);
                var reducedMessages = await reducer.ReduceAsync(ChatHistories);
                if (reducedMessages is not null)
                {
                    ChatHistories = [.. reducedMessages];
                }
            }

            var response = await _chatCompletionService.GetChatMessageContentAsync(
                ChatHistories,
                executionSettings: ServiceProvider.CreatePromptExecutionSettings(maxOutputToken, temperature).CreatePromptExecutionSettingsForJsonOutput<T>(ServiceProvider),
                kernel: Kernel);

            ChatHistories.Add(response);

            return JsonSerializer.Deserialize<T>(response.ToString());
        }

        public async Task HealthCheckAsync()
        {
            if (_chatCompletionService is null)
            {
                throw new InvalidOperationException("Chat completion service is not initialized.");
            }
            try
            {
                await _chatCompletionService.GetChatMessageContentAsync(
                    [
                        new ChatMessageContent
                        {
                            Role = AuthorRole.User,
                            Content = "Healthcheck message, please say `Hello World`"
                        }
                    ],
                    executionSettings: ServiceProvider.CreatePromptExecutionSettingsWithFunctionCalling(5, 0.2),
                    kernel: Kernel);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message, ex.InnerException);
            }
        }
    }
}
