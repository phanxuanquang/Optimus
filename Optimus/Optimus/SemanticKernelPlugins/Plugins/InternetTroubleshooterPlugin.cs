using Microsoft.SemanticKernel;
using Optimus.SemanticKernelPlugins.Interfaces;
using System.Threading.Tasks;

namespace Optimus.SemanticKernelPlugins.Plugins
{
    public class InternetTroubleshooterPlugin : IInternetTroubleshooterPlugin
    {
        [KernelFunction]
        public Task<string> TestInternetSpeed() => Task.FromResult("TestInternetSpeed not implemented yet");

        [KernelFunction]
        public Task<string> ResetDns() => Task.FromResult("ResetDns not implemented yet");
    }
}
