namespace PluginFramework;

public interface IPlugin
{
    void Execute();
    string Name { get; }
}