using Optimus.SemanticKernelPlugins.Plugins;
using System.Collections.Generic;

namespace Optimus.Helpers
{
    public static class Cache
    {
        public static string ApiKey { get; set; } = "";

        public static List<object> SemanticKernelPlugins { get; } =
        [
            new FileSystemPlugin(),
            new HardwareMonitorPlugin(),
            new InternetTroubleshooterPlugin(),
            new RepairToolsPlugin(),
            new SoftwareManagerPlugin(),
            new SystemDiagnosticsPlugin()
        ];
    }
}
