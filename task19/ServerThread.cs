namespace task19;

public class ServerThread
{
    private readonly Thread _thread;
    private readonly IScheduler _scheduler;
    private volatile bool _isRunning;
    private Action<ICommand, Exception>? _exceptionHandler;

    public ServerThread(IScheduler scheduler)
    {
        _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        _isRunning = true;
        _thread = new Thread(ProcessCommands)
        {
            IsBackground = true
        };
        _thread.Start();
    }

    public void SetExceptionHandler(Action<ICommand, Exception> handler)
    {
        _exceptionHandler = handler;
    }

    public void AddCommand(ICommand command)
    {
        if (!_isRunning)
            throw new InvalidOperationException("Поток остановлен");
        _scheduler.Add(command);
    }

    public void StopHard()
    {
        _isRunning = false;
        _thread.Join(5000);
    }

    public void StopSoft()
    {
        _isRunning = false;
    }

    public bool IsAlive => _thread.IsAlive;

    private void ProcessCommands()
    {
        while (_isRunning)
        {
            var command = _scheduler.Select();
            
            if (command != null)
            {
                try
                {
                    command.Execute();
                }
                catch (Exception ex)
                {
                    _exceptionHandler?.Invoke(command, ex);
                }
            }
            else
            {
                Thread.Sleep(1);
            }
        }
    }
}