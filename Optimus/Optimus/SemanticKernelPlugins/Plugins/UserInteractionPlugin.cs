using Microsoft.SemanticKernel;
using Optimus.SemanticKernelPlugins.Interfaces;
using System.Threading.Tasks;

namespace Optimus.SemanticKernelPlugins.Plugins
{
    public class UserInteractionPlugin : IUserInteractionPlugin
    {
        [KernelFunction]
        public Task<string> ConfirmBeforeFix(string actionDescription) => Task.FromResult($"ConfirmBeforeFix for: {actionDescription} not implemented yet");

        [KernelFunction]
        public Task<string> NotifyUser(string message) => Task.FromResult($"NotifyUser: {message} not implemented yet");
    }
}
