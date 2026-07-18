using System.Collections.Concurrent;
using System.Threading;

namespace task17;

public class ServerThread
{
    private readonly ConcurrentQueue<ICommand> _commandQueue = new();
    private readonly AutoResetEvent _commandAvailable = new(false);
    private Thread? _thread;
    private volatile bool _isRunning;
    private volatile bool _stopRequested;
    public Action<Exception, ICommand>? ExceptionHandler { get; set; }

    public void Start()
    {
        if (_thread != null && _thread.IsAlive)
            throw new InvalidOperationException("Поток уже запущен");

        _isRunning = true;
        _stopRequested = false;
        _thread = new Thread(ProcessCommands)
        {
            IsBackground = true
        };
        _thread.Start();
    }

    public void EnqueueCommand(ICommand command)
    {
        _commandQueue.Enqueue(command);
        _commandAvailable.Set();
    }

    public void Stop()
    {
        _isRunning = false;
        _stopRequested = true;
        _commandAvailable.Set();
        _thread?.Join();
    }

    private void ProcessCommands()
    {
        while (_isRunning)
        {
            if (_commandQueue.TryDequeue(out var command))
            {
                try
                {
                    if (command is HardStopCommand hardStop)
                    {
                        if (hardStop.TargetThread != this)
                            throw new InvalidOperationException("HardStop может выполняться только в целевом потоке");
                        
                        _isRunning = false;
                        _stopRequested = true;
                        _commandAvailable.Set();
                        return;
                    }
                    else if (command is SoftStopCommand softStop)
                    {
                        if (softStop.TargetThread != this)
                            throw new InvalidOperationException("SoftStop может выполняться только в целевом потоке");
                        
                        _stopRequested = true;
                    }
                    else
                    {
                        command.Execute();
                        
                        if (_stopRequested && _commandQueue.IsEmpty)
                            break;
                    }
                }
                catch (Exception ex) when (ExceptionHandler != null)
                {
                    ExceptionHandler(ex, command);
                }
                catch (Exception)
                {
                }
            }
            else
            {
                if (_stopRequested)
                    break;
                
                _commandAvailable.WaitOne(100);
            }
        }
    }

    public bool IsAlive => _thread?.IsAlive ?? false;
}