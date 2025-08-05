using Optimus.SemanticKernelPlugins;
using System.Collections.Generic;

namespace Optimus.Helpers
{
    public static class Cache
    {
        public static string ApiKey { get; set; } = "AIzaSyDYMIl_aacZrDbZJrjTWC6sRvMjlGcqQgU";

        public static List<object> SemanticKernelPlugins { get; } =
        [
            new FilePlugin(),
        ];
    }
}
