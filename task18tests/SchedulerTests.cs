using Xunit;
using task18;

namespace task18tests;

public class SchedulerTests
{
    [Fact]
    public void RoundRobinScheduler_AddAndSelect_WorksCorrectly()
    {
        var scheduler = new RoundRobinScheduler();
        var command = new SimpleCommand(() => { });
        
        scheduler.Add(command);
        Assert.True(scheduler.HasCommand());
        
        var selected = scheduler.Select();
        Assert.Same(command, selected);
        Assert.False(scheduler.HasCommand());
    }

    [Fact]
    public void RoundRobinScheduler_Select_ReturnsNull_WhenEmpty()
    {
        var scheduler = new RoundRobinScheduler();
        Assert.Null(scheduler.Select());
    }

    [Fact]
    public void RoundRobinScheduler_Add_Null_ThrowsException()
    {
        var scheduler = new RoundRobinScheduler();
        Assert.Throws<ArgumentNullException>(() => scheduler.Add(null!));
    }

    [Fact]
    public void ServerThread_ExecutesCommandsFromScheduler()
    {
        var results = new List<string>();
        var scheduler = new RoundRobinScheduler();
        var server = new ServerThread(scheduler);

        scheduler.Add(new SimpleCommand(() => results.Add("First")));
        scheduler.Add(new SimpleCommand(() => results.Add("Second")));

        Thread.Sleep(500);
        server.StopSoft();
        while (server.IsAlive) Thread.Sleep(50);

        Assert.Equal(new[] { "First", "Second" }, results);
    }

    [Fact]
    public void LongRunningCommand_ExecutesInSteps()
    {
        var command = new LongRunningCommand(5);
        
        for (int i = 0; i < 3; i++)
        {
            command.Execute();
        }
        
        Assert.Equal(3, command.CurrentStep);
        Assert.False(command.IsCompleted);
        
        command.Execute();
        command.Execute();
        
        Assert.True(command.IsCompleted);
    }

    [Fact]
    public void ServerThread_HandlesExceptionsWithScheduler()
    {
        Exception? capturedEx = null;
        var scheduler = new RoundRobinScheduler();
        var server = new ServerThread(scheduler);

        server.SetExceptionHandler((cmd, ex) => capturedEx = ex);

        scheduler.Add(new SimpleCommand(() => throw new InvalidOperationException("Test")));

        Thread.Sleep(500);
        server.StopSoft();
        while (server.IsAlive) Thread.Sleep(50);

        Assert.NotNull(capturedEx);
        Assert.IsType<InvalidOperationException>(capturedEx);
    }

    [Fact]
    public void ServerThread_StopHard_StopsImmediately()
    {
        var executedCount = 0;
        var scheduler = new RoundRobinScheduler();
        var server = new ServerThread(scheduler);

        for (int i = 0; i < 10; i++)
        {
            scheduler.Add(new SimpleCommand(() =>
            {
                Interlocked.Increment(ref executedCount);
                Thread.Sleep(100);
            }));
        }

        Thread.Sleep(250);
        server.StopHard();

        Assert.True(executedCount < 10);
        Assert.False(server.IsAlive);
    }

    [Fact]
    public void MultipleLongRunningCommands_InterleavedExecution()
    {
        var scheduler = new RoundRobinScheduler();
        var server = new ServerThread(scheduler);

        var cmd1 = new LongRunningCommand(5);
        var cmd2 = new LongRunningCommand(5);

        scheduler.Add(cmd1);
        scheduler.Add(cmd2);

        Thread.Sleep(300);
        server.StopSoft();
        while (server.IsAlive) Thread.Sleep(50);

        Assert.True(cmd1.CurrentStep > 0 || cmd2.CurrentStep > 0);
    }
}

