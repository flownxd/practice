using PluginFramework;

namespace Plugin2;

[PluginLoad]
public class AnotherPlugin : IPlugin
{
    public string Name => "AnotherPlugin";

    public void Execute()
    {
        Console.WriteLine("AnotherPlugin выполнен успешно!");
    }
}