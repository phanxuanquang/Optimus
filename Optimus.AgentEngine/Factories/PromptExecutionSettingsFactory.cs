using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Google;
using Microsoft.SemanticKernel.Connectors.Ollama;
using Microsoft.SemanticKernel.Connectors.Onnx;
using Optimus.Commons.Enums;

#pragma warning disable SKEXP0070

namespace Optimus.AgentEngine.Factories
{
    public static class PromptExecutionSettingsFactory
    {
        public static PromptExecutionSettings CreatePromptExecutionSettingsWithFunctionCalling(this AiServiceProvider serviceProvider, int maxOutputToken = 2048, double temperature = 1)
        {
            var promptExecutionSettings = new PromptExecutionSettings
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(
                    options: new FunctionChoiceBehaviorOptions
                    {
                        AllowConcurrentInvocation = false,
                        AllowParallelCalls = false,
                    },
                    autoInvoke: true)
            };

            promptExecutionSettings = serviceProvider switch
            {
                AiServiceProvider.Gemini => new GeminiPromptExecutionSettings
                {
                    ToolCallBehavior = GeminiToolCallBehavior.AutoInvokeKernelFunctions,
                    FunctionChoiceBehavior = promptExecutionSettings.FunctionChoiceBehavior,
                    MaxTokens = maxOutputToken,
                    Temperature = temperature
                },
                AiServiceProvider.Ollama => OllamaPromptExecutionSettings.FromExecutionSettings(promptExecutionSettings),
                AiServiceProvider.ONNX => OnnxRuntimeGenAIPromptExecutionSettings.FromExecutionSettings(promptExecutionSettings),
                _ => throw new NotImplementedException(),
            };

            return promptExecutionSettings!;
        }

        public static PromptExecutionSettings CreatePromptExecutionSettings(this AiServiceProvider serviceProvider, int maxOutputToken = 2048, double temperature = 1)
        {
            var promptExecutionSettings = new PromptExecutionSettings();

            promptExecutionSettings = serviceProvider switch
            {
                AiServiceProvider.Gemini => new GeminiPromptExecutionSettings
                {
                    MaxTokens = maxOutputToken,
                    Temperature = temperature
                },
                AiServiceProvider.Ollama => OllamaPromptExecutionSettings.FromExecutionSettings(promptExecutionSettings),
                AiServiceProvider.ONNX => OnnxRuntimeGenAIPromptExecutionSettings.FromExecutionSettings(promptExecutionSettings),
                _ => throw new NotImplementedException(),
            };

            return promptExecutionSettings!;
        }

        public static PromptExecutionSettings CreatePromptExecutionSettingsForJsonOutput<T>(this PromptExecutionSettings promptExecutionSettings, AiServiceProvider serviceProvider)
        {
            switch (serviceProvider)
            {
                case AiServiceProvider.Gemini:
                    GeminiPromptExecutionSettings.FromExecutionSettings(promptExecutionSettings).ResponseSchema = typeof(T);
                    GeminiPromptExecutionSettings.FromExecutionSettings(promptExecutionSettings).ResponseMimeType = "application/json";
                    break;
                default: throw new NotImplementedException($"The service provider {serviceProvider} does not support JSON output format.");
            }

            return promptExecutionSettings;
        }
    }
}
