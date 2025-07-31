using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Optimus.Commons.Enums;

#pragma warning disable SKEXP0070

namespace Optimus.AgentEngine.Factories
{
    public class KernelFactory
    {
        private readonly IKernelBuilder _kernelBuilder;
        public AiServiceProvider ServiceProvider { get; private set; }
        public KernelFactory()
        {
            _kernelBuilder = Kernel.CreateBuilder();
        }

        public KernelFactory WithPlugins(IEnumerable<object> plugins)
        {
            foreach (var plugin in plugins)
            {
                var isPluginAdded = _kernelBuilder.Plugins.Services.Any(s => s.ImplementationType == plugin.GetType());
                if (isPluginAdded)
                {
                    continue;
                }
                _kernelBuilder.Plugins.AddFromObject(plugin);
            }
            return this;
        }

        public KernelFactory WithService<TInterface, TImplementation>()
            where TInterface : class
            where TImplementation : class, TInterface
        {
            _kernelBuilder.Services.AddSingleton<TInterface, TImplementation>();
            return this;
        }

        public KernelFactory WithFunctionInvocationFilter<TFilter>(TFilter implementation)
            where TFilter : class, IFunctionInvocationFilter
        {
            _kernelBuilder.Services.AddSingleton<IFunctionInvocationFilter>(implementation);
            return this;
        }

        public KernelFactory WithAutoFunctionInvocationFilter<TFilter>(TFilter implementation)
           where TFilter : class, IAutoFunctionInvocationFilter
        {
            _kernelBuilder.Services.AddSingleton<IAutoFunctionInvocationFilter>(implementation);
            return this;
        }

        public Kernel Build()
        {
            return _kernelBuilder.Build();
        }

        #region AI Service Providers   

        public KernelFactory UseGoogleGeminiProvider(string apiKey, string modelId)
        {
            ServiceProvider = AiServiceProvider.Gemini;
            _kernelBuilder.AddGoogleAIGeminiChatCompletion(modelId, apiKey);
            return this;
        }
        public KernelFactory UseOllamaProvider(string endpoint, string modelId)
        {
            ServiceProvider = AiServiceProvider.Ollama;
            _kernelBuilder.AddOllamaChatCompletion(modelId, new Uri(endpoint));
            return this;
        }

        public KernelFactory UseOnnxProvider(string modelPath, string modelId)
        {
            ServiceProvider = AiServiceProvider.ONNX;
            _kernelBuilder.AddOnnxRuntimeGenAIChatCompletion(modelId, modelPath);
            return this;
        }

        #endregion
    }
}
