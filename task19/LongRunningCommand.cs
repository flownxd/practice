namespace task19;

public class LongRunningCommand : ICommand
{
    private readonly int _totalSteps;
    private int _currentStep;
    private readonly object _lock = new();
    private volatile bool _isCancelled;

    public LongRunningCommand(int totalSteps)
    {
        _totalSteps = totalSteps;
        _currentStep = 0;
    }

    public void Execute()
    {
        lock (_lock)
        {
            if (_isCancelled || _currentStep >= _totalSteps)
                return;
                
            Thread.Sleep(10);
            _currentStep++;
        }
    }

    public bool IsCompleted
    {
        get
        {
            lock (_lock)
                return _currentStep >= _totalSteps;
        }
    }

    public int CurrentStep
    {
        get
        {
            lock (_lock)
                return _currentStep;
        }
    }

    public void Cancel()
    {
        _isCancelled = true;
    }

    public bool IsCancelled => _isCancelled;
    
    public void Requeue(IScheduler scheduler)
    {
        if (!IsCompleted && !_isCancelled)
        {
            scheduler.Add(this);
        }
    }
}