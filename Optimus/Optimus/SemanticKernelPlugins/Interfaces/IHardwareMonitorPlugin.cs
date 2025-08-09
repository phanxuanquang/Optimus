using System.Threading.Tasks;

namespace Optimus.SemanticKernelPlugins.Interfaces
{
    public interface IHardwareMonitorPlugin
    {
        Task<string> GetCpuTemperature();
        Task<string> GetGpuTemperature();
    }
}
