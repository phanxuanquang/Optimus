using System.Threading.Tasks;

namespace Optimus.SemanticKernelPlugins.Interfaces
{
    public interface ILogAndHistoryPlugin
    {
        Task<string> SaveActionLog(string action, string result);
        Task<string> GetLatestLog();
    }
}
