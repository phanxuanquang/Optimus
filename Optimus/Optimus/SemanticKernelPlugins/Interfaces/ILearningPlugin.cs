using System.Threading.Tasks;

namespace Optimus.SemanticKernelPlugins.Interfaces
{
    public interface ILearningPlugin
    {
        Task<string> SaveSuccessfulFix(string problem, string solution);
        Task<string> TrainOnNewCase(string problem, string userFix);
    }
}
