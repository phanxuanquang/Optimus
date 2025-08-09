using Microsoft.SemanticKernel;
using Optimus.SemanticKernelPlugins.Interfaces;
using System.Threading.Tasks;

namespace Optimus.SemanticKernelPlugins.Plugins
{
    public class FileSystemPlugin : IFileSystemPlugin
    {
        [KernelFunction]
        public Task<string> FindLargeFiles(string folder, int sizeThresholdMB) => Task.FromResult($"FindLargeFiles in {folder} not implemented yet");

        [KernelFunction]
        public Task<string> CleanTempFolders() => Task.FromResult("CleanTempFolders not implemented yet");
    }
}
