using System.Threading.Tasks;

namespace Optimus.SemanticKernelPlugins.Interfaces
{
    public interface ISoftwareManagerPlugin
    {
        Task<string> ListInstalledApps();
        Task<string> UninstallApp(string appName);
    }
}
