using Optimus.SemanticKernelPlugins;
using System.Collections.Generic;

namespace Optimus.Helpers
{
    public static class Cache
    {
        public static string ApiKey { get; set; } = "";

        public static List<object> SemanticKernelPlugins { get; } =
        [
            new FilePlugin(),
        ];
    }
}
