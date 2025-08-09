using System.Threading.Tasks;

namespace Optimus.SemanticKernelPlugins.Interfaces
{
    public interface ISystemDiagnosticsPlugin
    {
        Task<string> GetSystemInfo();
        Task<string> CheckSystemErrors();
        Task<string> DiagnoseSlowPerformance();
    }
}
