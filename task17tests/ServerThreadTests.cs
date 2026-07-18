using System.Threading;
using task17;
using Xunit;

namespace task17tests;

public class ServerThreadTests
{
    [Fact]
    public void ServerThread_Start_CreatesRunningThread()
    {
        var serverThread = new ServerThread();
        serverThread.Start();
        
        Assert.True(serverThread.IsAlive);
        
        serverThread.Stop();
    }

    [Fact]
    public void ServerThread_EnqueueCommand_ExecutesCommand()
    {
        var executed = false;
        var serverThread = new ServerThread();
        serverThread.Start();

        var command = new TestCommand(() => executed = true);
        serverThread.EnqueueCommand(command);

        Thread.Sleep(200);
        
        Assert.True(executed);
        
        serverThread.Stop();
    }

    [Fact]
    public void HardStopCommand_StopsThreadImmediately()
    {
        var serverThread = new ServerThread();
        serverThread.Start();

        var command1Started = new ManualResetEventSlim(false);
        var command1CanFinish = new ManualResetEventSlim(false);
        var command2Executed = false;

        serverThread.EnqueueCommand(new TestCommand(() => 
        {
            command1Started.Set();
            command1CanFinish.Wait();
        }));

        Assert.True(command1Started.Wait(1000));

        var hardStop = new HardStopCommand(serverThread);
        serverThread.EnqueueCommand(hardStop);
        
        serverThread.EnqueueCommand(new TestCommand(() => 
        {
            command2Executed = true;
        }));

        command1CanFinish.Set();

        Thread.Sleep(200);

        Assert.False(command2Executed);
        Assert.False(serverThread.IsAlive);
    }

    [Fact]
    public void SoftStopCommand_StopsThreadAfterQueueEmpty()
    {
        var serverThread = new ServerThread();
        serverThread.Start();

        var commandsExecuted = 0;

        for (int i = 0; i < 3; i++)
        {
            serverThread.EnqueueCommand(new TestCommand(() => 
            {
                Interlocked.Increment(ref commandsExecuted);
            }));
        }

        var softStop = new SoftStopCommand(serverThread);
        serverThread.EnqueueCommand(softStop);

        Thread.Sleep(300);
        
        Assert.Equal(3, commandsExecuted);
        Assert.False(serverThread.IsAlive);
    }

    [Fact]
    public void HardStopCommand_ThrowsWhenExecutedInWrongThread()
    {
        var serverThread1 = new ServerThread();
        var serverThread2 = new ServerThread();
        serverThread1.Start();
        serverThread2.Start();

        var exceptionThrown = false;
        serverThread2.ExceptionHandler = (ex, cmd) =>
        {
            if (ex is InvalidOperationException)
                exceptionThrown = true;
        };

        var hardStop = new HardStopCommand(serverThread1);
        serverThread2.EnqueueCommand(hardStop);

        Thread.Sleep(200);
        
        Assert.True(exceptionThrown);
        
        serverThread1.Stop();
        serverThread2.Stop();
    }

    [Fact]
    public void ServerThread_ExceptionHandler_CatchesException()
    {
        var exceptionHandled = false;
        var serverThread = new ServerThread();
        serverThread.ExceptionHandler = (ex, cmd) =>
        {
            exceptionHandled = true;
        };
        serverThread.Start();

        var command = new TestCommand(() => throw new InvalidOperationException("Test"));
        serverThread.EnqueueCommand(command);

        Thread.Sleep(200);
        
        Assert.True(exceptionHandled);
        
        serverThread.Stop();
    }

    private class TestCommand : ICommand
    {
        private readonly Action _action;

        public TestCommand(Action action)
        {
            _action = action;
        }

        public void Execute() => _action();
    }
}