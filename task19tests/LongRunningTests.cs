using Xunit;
using task19;

namespace task19tests;

public class LongRunningTests
{
    [Fact]
    public void TestCommand_ExecutesMultipleTimes()
    {
        var scheduler = new RoundRobinScheduler();
        var command = new TestCommand(1, 3, scheduler);
        
        command.Execute();
        Assert.Equal(1, command.Counter);
        Assert.False(command.IsCompleted);
        
        command.Execute();
        Assert.Equal(2, command.Counter);
        Assert.False(command.IsCompleted);
        
        command.Execute();
        Assert.Equal(3, command.Counter);
        Assert.True(command.IsCompleted);
    }

    [Fact]
    public void LongRunningCommand_ProgressTracking()
    {
        var command = new LongRunningCommand(5);
        
        for (int i = 0; i < 3; i++)
        {
            command.Execute();
        }
        
        Assert.Equal(3, command.CurrentStep);
        Assert.False(command.IsCompleted);
    }

    [Fact]
    public void LongRunningCommand_CanBeCancelled()
    {
        var command = new LongRunningCommand(10);
        
        command.Execute();
        command.Execute();
        Assert.Equal(2, command.CurrentStep);
        
        command.Cancel();
        command.Execute();
        command.Execute();
        
        Assert.Equal(2, command.CurrentStep);
        Assert.True(command.IsCancelled);
    }

    [Fact]
    public void ServerThread_ProcessesMultipleTestCommands()
    {
        var scheduler = new RoundRobinScheduler();
        var server = new ServerThread(scheduler);
        var commands = new List<TestCommand>();

        for (int i = 1; i <= 5; i++)
        {
            var cmd = new TestCommand(i, 3, scheduler);
            commands.Add(cmd);
            scheduler.Add(cmd);
        }

        Thread.Sleep(2000);
        server.StopHard();

        foreach (var cmd in commands)
        {
            Assert.Equal(3, cmd.Counter);
            Assert.True(cmd.IsCompleted);
        }
    }

    [Fact]
    public void ServerThread_HardStop_StopsExecution()
    {
        var scheduler = new RoundRobinScheduler();
        var server = new ServerThread(scheduler);

        for (int i = 0; i < 10; i++)
        {
            scheduler.Add(new LongRunningCommand(100));
        }

        Thread.Sleep(500);
        server.StopHard();

        Assert.False(server.IsAlive);
    }

    [Fact]
    public void RoundRobinScheduler_FairExecution()
    {
        var scheduler = new RoundRobinScheduler();
        var server = new ServerThread(scheduler);
        var commands = new List<LongRunningCommand>();

        for (int i = 0; i < 3; i++)
        {
            var cmd = new LongRunningCommand(5);
            commands.Add(cmd);
            scheduler.Add(cmd);
        }

        Thread.Sleep(500);
        
        var steps = commands.Select(c => c.CurrentStep).ToList();
        Assert.All(steps, step => Assert.True(step > 0));
        
        server.StopSoft();
        while (server.IsAlive) Thread.Sleep(50);
    }
}