using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PluginFramework;

public static class PluginLoader
{
    public static List<PluginInfo> DiscoverPlugins(string directory)
    {
        var plugins = new List<PluginInfo>();
        
        if (!Directory.Exists(directory))
        {
            throw new DirectoryNotFoundException($"Директория '{directory}' не найдена");
        }

        var dllFiles = Directory.GetFiles(directory, "*.dll");
        
        foreach (var dllFile in dllFiles)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(dllFile);
                var pluginTypes = assembly.GetTypes()
                    .Where(t => typeof(IPlugin).IsAssignableFrom(t) 
                             && !t.IsInterface 
                             && !t.IsAbstract);

                foreach (var type in pluginTypes)
                {
                    var attr = type.GetCustomAttribute<PluginLoadAttribute>();
                    if (attr != null)
                    {
                        plugins.Add(new PluginInfo
                        {
                            Type = type,
                            Name = type.Name,
                            Dependencies = attr.Dependencies,
                            Assembly = assembly
                        });
                    }
                }
            }
            catch
            {
            }
        }

        return plugins;
    }

    public static List<PluginInfo> SortByDependencies(List<PluginInfo> plugins)
    {
        var sorted = new List<PluginInfo>();
        var visited = new HashSet<string>();
        var tempMark = new HashSet<string>();

        void Visit(PluginInfo plugin)
        {
            if (tempMark.Contains(plugin.Name))
            {
                throw new Exception($"Обнаружена циклическая зависимость: {plugin.Name}");
            }

            if (visited.Contains(plugin.Name))
            {
                return;
            }

            tempMark.Add(plugin.Name);

            foreach (var depName in plugin.Dependencies)
            {
                var dep = plugins.FirstOrDefault(p => p.Name == depName);
                if (dep != null)
                {
                    Visit(dep);
                }
            }

            tempMark.Remove(plugin.Name);
            visited.Add(plugin.Name);
            sorted.Add(plugin);
        }

        foreach (var plugin in plugins)
        {
            if (!visited.Contains(plugin.Name))
            {
                Visit(plugin);
            }
        }

        return sorted;
    }

    public static void LoadAndExecutePlugins(List<PluginInfo> plugins)
    {
        foreach (var plugin in plugins)
        {
            try
            {
                Console.WriteLine($"\nЗагрузка плагина: {plugin.Name}");
                IPlugin instance = plugin.CreateInstance();
                Console.WriteLine($"Выполнение плагина: {instance.Name}");
                instance.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке плагина {plugin.Name}: {ex.Message}");
            }
        }
    }
}