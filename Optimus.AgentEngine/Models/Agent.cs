using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimus.AgentEngine.Models
{
    public class Agent
    {
        public string CodeName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string SystemInstruction { get; set; } = string.Empty;
        public List<IPlugin> Plugins { get; set; } = new();

        private Kernel _kernel;

        public Agent(Kernel kernel)
        {
            _kernel = kernel;
        }

        public async Task<string> InvokeAsync(string userInput, Dictionary<string, object>? context = null)
        {
            string systemPrompt = $"{SystemInstruction}\nUser: {userInput}";

            foreach (var plugin in Plugins)
            {
                if (plugin.CanHandle(userInput))
                {
                    return await plugin.InvokeAsync(userInput, _kernel, context);
                }
            }

            // Nếu không plugin nào xử lý, dùng default model
            var function = _kernel.CreateFunctionFromPrompt(systemPrompt, functionName: $"{CodeName}_default");
            return await _kernel.InvokeAsync(function);
        }
    }

}
