using Microsoft.SemanticKernel;
using Optimus.SemanticKernelPlugins.Interfaces;
using System.Threading.Tasks;

namespace Optimus.SemanticKernelPlugins.Plugins
{
    public class SystemDiagnosticsPlugin : ISystemDiagnosticsPlugin
    {
        [KernelFunction]
        public Task<string> GetSystemInfo() => Task.FromResult("System info not implemented yet");

        [KernelFunction]
        public Task<string> CheckSystemErrors() => Task.FromResult("CheckSystemErrors not implemented yet");

        [KernelFunction]
        public Task<string> DiagnoseSlowPerformance() => Task.FromResult("DiagnoseSlowPerformance not implemented yet");
    }
}
