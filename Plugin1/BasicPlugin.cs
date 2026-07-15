using PluginFramework;

namespace Plugin1;

[PluginLoad]
public class BasicPlugin : IPlugin
{
    public string Name => "BasicPlugin";

    public void Execute()
    {
        Console.WriteLine("BasicPlugin выполнен успешно!");
    }
}