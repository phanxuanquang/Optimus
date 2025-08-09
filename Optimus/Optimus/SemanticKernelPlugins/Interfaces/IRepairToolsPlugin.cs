using System.Threading.Tasks;

namespace Optimus.SemanticKernelPlugins.Interfaces
{
    public interface IRepairToolsPlugin
    {
        Task<string> RunSfcScan();
        Task<string> RunDismRestore();
    }
}
