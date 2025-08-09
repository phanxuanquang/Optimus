using Microsoft.SemanticKernel;
using Optimus.SemanticKernelPlugins.Interfaces;
using System.Threading.Tasks;

namespace Optimus.SemanticKernelPlugins.Plugins
{
    public class RepairToolsPlugin : IRepairToolsPlugin
    {
        [KernelFunction]
        public Task<string> RunSfcScan() => Task.FromResult("RunSfcScan not implemented yet");

        [KernelFunction]
        public Task<string> RunDismRestore() => Task.FromResult("RunDismRestore not implemented yet");
    }
}
