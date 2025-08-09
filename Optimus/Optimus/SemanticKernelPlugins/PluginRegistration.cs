using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Optimus.SemanticKernelPlugins
{
    public record PluginScanResult(Type ImplementationType, Type? ServiceInterface, string SkillName);

    public static class PluginRegistration
    {
        public static IEnumerable<PluginScanResult> ScanPluginsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsPublic);

            foreach (var t in types)
            {
                var methods = t.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                bool hasKernelFunction = methods.Any(m =>
                    m.CustomAttributes.Any(attr => string.Equals(attr.AttributeType.Name, "KernelFunctionAttribute", StringComparison.OrdinalIgnoreCase))
                );

                if (!hasKernelFunction)
                    continue;

                // Try to find a single interface that looks like plugin interface (I<Name> matching)
                Type? serviceInterface = t.GetInterfaces()
                    .FirstOrDefault(i => i.Name.Equals("I" + t.Name, StringComparison.OrdinalIgnoreCase));

                // Fallback: take first interface if exists (optional)
                if (serviceInterface == null && t.GetInterfaces().Any())
                {
                    serviceInterface = t.GetInterfaces().First();
                }

                // Skill name: mặc định dùng tên class không có suffix "Plugin"
                var skillName = t.Name.EndsWith("Plugin", StringComparison.OrdinalIgnoreCase)
                    ? t.Name.Substring(0, t.Name.Length - "Plugin".Length)
                    : t.Name;

                yield return new PluginScanResult(t, serviceInterface, skillName);
            }
        }

        public static void RegisterPluginsToDI(this IServiceCollection services, Assembly assembly, ILogger? logger = null)
        {
            var plugins = ScanPluginsFromAssembly(assembly).ToList();

            foreach (var p in plugins)
            {
                if (p.ServiceInterface != null && p.ServiceInterface.IsAssignableFrom(p.ImplementationType))
                {
                    services.AddTransient(p.ServiceInterface, p.ImplementationType);
                    logger?.LogInformation("Registered {Impl} as {Iface}", p.ImplementationType.FullName, p.ServiceInterface.FullName);
                }
                else
                {
                    services.AddTransient(p.ImplementationType);
                    logger?.LogInformation("Registered concrete {Impl}", p.ImplementationType.FullName);
                }
            }
        }

        public static async Task ImportPluginsToKernelAsync(this IServiceProvider serviceProvider, object kernel, Assembly assembly, ILogger? logger = null)
        {
            if (kernel == null) throw new ArgumentNullException(nameof(kernel));

            var plugins = ScanPluginsFromAssembly(assembly).ToList();

            foreach (var p in plugins)
            {
                object? instance = null;
                if (p.ServiceInterface != null)
                {
                    instance = serviceProvider.GetService(p.ServiceInterface);
                }
                if (instance == null)
                {
                    instance = serviceProvider.GetService(p.ImplementationType);
                }

                if (instance == null)
                {
                    logger?.LogWarning("Cannot resolve plugin instance for {Type}. Did you register it in DI?", p.ImplementationType.FullName);
                    continue;
                }

                var importMethodCandidates = new[] {
                    "ImportSkill",                // common
                    "ImportSkillFromObject",      // variant
                    "RegisterSkill",              // another variant
                    "ImportSkillFromTypes",       // less common
                };

                bool imported = false;
                var kernelType = kernel.GetType();

                foreach (var methodName in importMethodCandidates)
                {
                    var methods = kernelType
                        .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                        .Where(m => string.Equals(m.Name, methodName, StringComparison.OrdinalIgnoreCase));

                    foreach (var m in methods)
                    {
                        var parameters = m.GetParameters();
                        try
                        {
                            if (parameters.Length >= 2 &&
                                parameters[0].ParameterType == typeof(object) &&
                                parameters[1].ParameterType == typeof(string))
                            {
                                m.Invoke(kernel, [instance, p.SkillName]);
                                imported = true;
                                logger?.LogInformation("Imported plugin {Plugin} into kernel via {Method}", p.ImplementationType.Name, methodName);
                                break;
                            }

                            if (parameters.Length >= 2 && parameters[0].ParameterType.IsAssignableFrom(p.ImplementationType) &&
                                parameters[1].ParameterType == typeof(string))
                            {
                                m.Invoke(kernel, new object[] { instance, p.SkillName });
                                imported = true;
                                logger?.LogInformation("Imported plugin {Plugin} into kernel via {Method} (assignable)", p.ImplementationType.Name, methodName);
                                break;
                            }
                        }
                        catch (TargetInvocationException tie)
                        {
                            logger?.LogWarning(tie, "Import method {Method} threw when importing {Plugin}. Trying next.", methodName, p.ImplementationType.Name);
                        }
                    }

                    if (imported) break;
                }

                if (!imported)
                {
                    logger?.LogWarning("Cannot import plugin {Plugin} into kernel for skill '{SkillName}'.", p.ImplementationType.FullName, p.SkillName);
                }

                await Task.Yield();
            }
        }
    }
}
