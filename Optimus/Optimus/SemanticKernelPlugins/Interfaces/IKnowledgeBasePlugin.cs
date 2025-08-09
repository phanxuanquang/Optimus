using System.Threading.Tasks;

namespace Optimus.SemanticKernelPlugins.Interfaces
{
    public interface IKnowledgeBasePlugin
    {
        Task<string> LookupErrorCode(string code);
        Task<string> SearchSolution(string query);
    }
}
