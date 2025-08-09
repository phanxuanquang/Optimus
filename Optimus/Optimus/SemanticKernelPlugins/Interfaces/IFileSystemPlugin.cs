using System.Threading.Tasks;

namespace Optimus.SemanticKernelPlugins.Interfaces
{
    public interface IFileSystemPlugin
    {
        Task<string> FindLargeFiles(string folder, int sizeThresholdMB);
        Task<string> CleanTempFolders();
    }
}
