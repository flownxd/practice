using PluginFramework;

Console.WriteLine("Система плагинов\n");

try
{
    string pluginsDir = Path.Combine(AppContext.BaseDirectory, "Plugins");
    
    if (!Directory.Exists(pluginsDir))
    {
        Directory.CreateDirectory(pluginsDir);
        Console.WriteLine($"Директория Plugins создана: {pluginsDir}");
        Console.WriteLine("Скопируйте DLL с плагинами в эту директорию");
        return;
    }

    Console.WriteLine($"Поиск плагинов в: {pluginsDir}\n");

    var plugins = PluginLoader.DiscoverPlugins(pluginsDir);
    
    if (plugins.Count == 0)
    {
        Console.WriteLine("Плагины не найдены");
        return;
    }

    Console.WriteLine($"Найдено плагинов: {plugins.Count}");
    foreach (var plugin in plugins)
    {
        string deps = plugin.Dependencies.Length > 0 
            ? $" (зависимости: {string.Join(", ", plugin.Dependencies)})" 
            : "";
        Console.WriteLine($"  - {plugin.Name}{deps}");
    }

    Console.WriteLine("\nСортировка по зависимостям");
    var sortedPlugins = PluginLoader.SortByDependencies(plugins);

    Console.WriteLine("\nПорядок загрузки:");
    for (int i = 0; i < sortedPlugins.Count; i++)
    {
        Console.WriteLine($"  {i + 1}. {sortedPlugins[i].Name}");
    }

    Console.WriteLine("\nЗагрузка и выполнение плагинов");
    PluginLoader.LoadAndExecutePlugins(sortedPlugins);

    Console.WriteLine("\nВсе плагины выполнены");
}
catch (Exception ex)
{
    Console.WriteLine($"Критическая ошибка: {ex.Message}");
}