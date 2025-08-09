using Microsoft.SemanticKernel;
using Optimus.SemanticKernelPlugins.Interfaces;
using System.Threading.Tasks;

namespace Optimus.SemanticKernelPlugins.Plugins
{
    public class KnowledgeBasePlugin : IKnowledgeBasePlugin
    {
        [KernelFunction]
        public Task<string> LookupErrorCode(string code) => Task.FromResult($"LookupErrorCode for {code} not implemented yet");

        [KernelFunction]
        public Task<string> SearchSolution(string query) => Task.FromResult($"SearchSolution for '{query}' not implemented yet");
    }
}
