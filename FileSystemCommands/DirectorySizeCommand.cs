using CommandLib;

namespace FileSystemCommands;

public class DirectorySizeCommand : ICommand
{
    private readonly string _directoryPath;
    private long _size;

    public DirectorySizeCommand(string directoryPath)
    {
        _directoryPath = directoryPath;
    }

    public void Execute()
    {
        if (!Directory.Exists(_directoryPath))
            throw new DirectoryNotFoundException($"Каталог '{_directoryPath}' не найден");

        _size = CalculateDirectorySize(_directoryPath);
        Console.WriteLine($"Размер каталога '{_directoryPath}': {_size} байт");
    }

    public long GetSize()
    {
        if (_size == 0)
        {
            _size = CalculateDirectorySize(_directoryPath);
        }
        return _size;
    }

    private long CalculateDirectorySize(string path)
    {
        long totalSize = 0;
        
        try
        {
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                try
                {
                    var fileInfo = new FileInfo(file);
                    totalSize += fileInfo.Length;
                }
                catch { }
            }

            var directories = Directory.GetDirectories(path);
            foreach (var dir in directories)
            {
                try
                {
                    totalSize += CalculateDirectorySize(dir);
                }
                catch { }
            }
        }
        catch { }

        return totalSize;
    }
}