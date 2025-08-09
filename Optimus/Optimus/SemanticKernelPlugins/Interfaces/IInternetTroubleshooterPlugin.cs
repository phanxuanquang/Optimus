using System.Threading.Tasks;

namespace Optimus.SemanticKernelPlugins.Interfaces
{
    public interface IInternetTroubleshooterPlugin
    {
        Task<string> TestInternetSpeed();
        Task<string> ResetDns();
    }
}
