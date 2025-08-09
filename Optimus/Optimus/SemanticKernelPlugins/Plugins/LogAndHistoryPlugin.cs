using Microsoft.SemanticKernel;
using Optimus.SemanticKernelPlugins.Interfaces;
using System.Threading.Tasks;

namespace Optimus.SemanticKernelPlugins.Plugins
{
    public class LogAndHistoryPlugin : ILogAndHistoryPlugin
    {
        [KernelFunction]
        public Task<string> SaveActionLog(string action, string result) => Task.FromResult($"SaveActionLog not implemented yet: {action}");

        [KernelFunction]
        public Task<string> GetLatestLog() => Task.FromResult("GetLatestLog not implemented yet");
    }
}
