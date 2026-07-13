using System;
using System.Reflection;

namespace PluginFramework;

public class PluginInfo
{
    public Type Type { get; set; }
    public string Name { get; set; }
    public string[] Dependencies { get; set; }
    public Assembly Assembly { get; set; }

    public IPlugin CreateInstance()
    {
        return (IPlugin)Activator.CreateInstance(Type);
    }
}