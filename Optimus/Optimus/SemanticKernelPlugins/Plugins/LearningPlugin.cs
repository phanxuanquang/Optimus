using Microsoft.SemanticKernel;
using Optimus.SemanticKernelPlugins.Interfaces;
using System.Threading.Tasks;

namespace Optimus.SemanticKernelPlugins.Plugins
{
    public class LearningPlugin : ILearningPlugin
    {
        [KernelFunction]
        public Task<string> SaveSuccessfulFix(string problem, string solution) => Task.FromResult($"SaveSuccessfulFix not implemented yet: {problem}");

        [KernelFunction]
        public Task<string> TrainOnNewCase(string problem, string userFix) => Task.FromResult($"TrainOnNewCase not implemented yet: {problem}");
    }
}
