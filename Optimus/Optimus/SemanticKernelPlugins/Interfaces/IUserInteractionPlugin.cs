using System.Threading.Tasks;

namespace Optimus.SemanticKernelPlugins.Interfaces
{
    public interface IUserInteractionPlugin
    {
        Task<string> ConfirmBeforeFix(string actionDescription);
        Task<string> NotifyUser(string message);
    }
}
