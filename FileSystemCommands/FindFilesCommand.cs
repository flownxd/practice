using CommandLib;

namespace FileSystemCommands;

[DisplayName("Команда поиска файлов")]
public class FindFilesCommand : ICommand
{
    private readonly string _directoryPath;
    private readonly string _searchPattern;
    private List<string> _foundFiles;

    public FindFilesCommand(string directoryPath, string searchPattern)
    {
        _directoryPath = directoryPath;
        _searchPattern = searchPattern;
        _foundFiles = new List<string>();
    }

    [DisplayName("Выполнить поиск")]
    public void Execute()
    {
        if (!Directory.Exists(_directoryPath))
            throw new DirectoryNotFoundException($"Каталог '{_directoryPath}' не найден");

        _foundFiles = FindFiles(_directoryPath, _searchPattern);
        
        Console.WriteLine($"Найдено файлов: {_foundFiles.Count}");
        foreach (var file in _foundFiles)
        {
            Console.WriteLine($"  - {file}");
        }
    }

    public List<string> GetFoundFiles()
    {
        if (_foundFiles == null || _foundFiles.Count == 0)
        {
            _foundFiles = FindFiles(_directoryPath, _searchPattern);
        }
        return _foundFiles;
    }

    private List<string> FindFiles(string path, string pattern)
    {
        var result = new List<string>();
        
        try
        {
            var files = Directory.GetFiles(path, pattern);
            result.AddRange(files);

            var directories = Directory.GetDirectories(path);
            foreach (var dir in directories)
            {
                try
                {
                    result.AddRange(FindFiles(dir, pattern));
                }
                catch { }
            }
        }
        catch { }

        return result;
    }
}