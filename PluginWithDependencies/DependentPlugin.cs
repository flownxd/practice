using PluginFramework;

namespace PluginWithDependencies;

[PluginLoad("BasicPlugin", "AnotherPlugin")]
public class DependentPlugin : IPlugin
{
    public string Name => "DependentPlugin";

    public void Execute()
    {
        Console.WriteLine("DependentPlugin выполнен успешно!");
        Console.WriteLine("Зависимости: BasicPlugin, AnotherPlugin загружены");
    }
}