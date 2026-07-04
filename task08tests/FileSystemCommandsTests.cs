using Xunit;
using System.IO;
using FileSystemCommands;

namespace task08tests;

public class FileSystemCommandsTests
{
    [Fact]
    public void DirectorySizeCommand_ShouldCalculateSize()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir_" + Guid.NewGuid());
        try
        {
            Directory.CreateDirectory(testDir);
            File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
            File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");

            var command = new DirectorySizeCommand(testDir);
            command.Execute();
            
            var size = command.GetSize();
            Assert.Equal(10, size);
        }
        finally
        {
            if (Directory.Exists(testDir))
                Directory.Delete(testDir, true);
        }
    }

    [Fact]
    public void FindFilesCommand_ShouldFindMatchingFiles()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir_" + Guid.NewGuid());
        try
        {
            Directory.CreateDirectory(testDir);
            File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
            File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");
            File.WriteAllText(Path.Combine(testDir, "readme.txt"), "Readme");

            var command = new FindFilesCommand(testDir, "*.txt");
            command.Execute();
            
            var foundFiles = command.GetFoundFiles();
            Assert.Equal(2, foundFiles.Count);
        }
        finally
        {
            if (Directory.Exists(testDir))
                Directory.Delete(testDir, true);
        }
    }

    [Fact]
    public void DirectorySizeCommand_ThrowsForNonExistentDirectory()
    {
        var command = new DirectorySizeCommand("C:\\NonExistentDirectory12345");
        Assert.Throws<DirectoryNotFoundException>(() => command.Execute());
    }

    [Fact]
    public void FindFilesCommand_ThrowsForNonExistentDirectory()
    {
        var command = new FindFilesCommand("C:\\NonExistentDirectory12345", "*.txt");
        Assert.Throws<DirectoryNotFoundException>(() => command.Execute());
    }

    [Fact]
    public void FindFilesCommand_FindsNoFilesWhenNoMatches()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir_" + Guid.NewGuid());
        try
        {
            Directory.CreateDirectory(testDir);
            File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
            File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

            var command = new FindFilesCommand(testDir, "*.pdf");
            command.Execute();
            
            var foundFiles = command.GetFoundFiles();
            Assert.Empty(foundFiles);
        }
        finally
        {
            if (Directory.Exists(testDir))
                Directory.Delete(testDir, true);
        }
    }

    [Fact]
    public void DirectorySizeCommand_HandlesEmptyDirectory()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir_" + Guid.NewGuid());
        try
        {
            Directory.CreateDirectory(testDir);

            var command = new DirectorySizeCommand(testDir);
            command.Execute();
            
            var size = command.GetSize();
            Assert.Equal(0, size);
        }
        finally
        {
            if (Directory.Exists(testDir))
                Directory.Delete(testDir, true);
        }
    }

    [Fact]
    public void FindFilesCommand_FindsFilesRecursively()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir_" + Guid.NewGuid());
        var subDir = Path.Combine(testDir, "SubDir");
        try
        {
            Directory.CreateDirectory(subDir);
            File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
            File.WriteAllText(Path.Combine(subDir, "file2.txt"), "Text");

            var command = new FindFilesCommand(testDir, "*.txt");
            command.Execute();
            
            var foundFiles = command.GetFoundFiles();
            Assert.Equal(2, foundFiles.Count);
        }
        finally
        {
            if (Directory.Exists(testDir))
                Directory.Delete(testDir, true);
        }
    }

    [Fact]
    public void DirectorySizeCommand_HandlesSubdirectories()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir_" + Guid.NewGuid());
        var subDir = Path.Combine(testDir, "SubDir");
        try
        {
            Directory.CreateDirectory(subDir);
            File.WriteAllText(Path.Combine(testDir, "file1.txt"), "12345");
            File.WriteAllText(Path.Combine(subDir, "file2.txt"), "12345");

            var command = new DirectorySizeCommand(testDir);
            command.Execute();
            
            var size = command.GetSize();
            Assert.Equal(10, size);
        }
        finally
        {
            if (Directory.Exists(testDir))
                Directory.Delete(testDir, true);
        }
    }
}