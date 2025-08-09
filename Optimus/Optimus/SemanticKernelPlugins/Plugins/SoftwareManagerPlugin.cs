using Microsoft.SemanticKernel;
using Optimus.SemanticKernelPlugins.Interfaces;
using System.Threading.Tasks;

namespace Optimus.SemanticKernelPlugins.Plugins
{
    public class SoftwareManagerPlugin : ISoftwareManagerPlugin
    {
        [KernelFunction]
        public Task<string> ListInstalledApps() => Task.FromResult("ListInstalledApps not implemented yet");

        [KernelFunction]
        public Task<string> UninstallApp(string appName) => Task.FromResult($"UninstallApp {appName} not implemented yet");
    }
}
