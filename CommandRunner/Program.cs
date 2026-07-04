using CommandLib;
using FileSystemCommands;
using System.Reflection;

var testDir = Path.Combine(Path.GetTempPath(), "TestDir_" + Guid.NewGuid());
Directory.CreateDirectory(testDir);

File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Hello World");
File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log data");
File.WriteAllText(Path.Combine(testDir, "readme.txt"), "README");

var sizeCommand = new DirectorySizeCommand(testDir);
sizeCommand.Execute();

var findCommand = new FindFilesCommand(testDir, "*.txt");
findCommand.Execute();

Directory.Delete(testDir, true);

var assemblyPath = Path.Combine(AppContext.BaseDirectory, "FileSystemCommands.dll");

if (File.Exists(assemblyPath))
{
    Assembly assembly = Assembly.LoadFrom(assemblyPath);
    Console.WriteLine($"Загружена сборка: {assembly.GetName().Name}");
    
    var commandTypes = assembly.GetTypes()
        .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsInterface);
    
    foreach (var type in commandTypes)
    {
        Console.WriteLine($"Найден класс: {type.Name}");
    }
}