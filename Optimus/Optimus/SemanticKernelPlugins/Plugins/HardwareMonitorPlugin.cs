using Microsoft.SemanticKernel;
using Optimus.SemanticKernelPlugins.Interfaces;
using System.Threading.Tasks;

namespace Optimus.SemanticKernelPlugins.Plugins
{
    public class HardwareMonitorPlugin : IHardwareMonitorPlugin
    {
        [KernelFunction]
        public Task<string> GetCpuTemperature() => Task.FromResult("CPU temperature not implemented yet");

        [KernelFunction]
        public Task<string> GetGpuTemperature() => Task.FromResult("GPU temperature not implemented yet");
    }
}
