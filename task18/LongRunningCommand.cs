namespace task18;

public class LongRunningCommand : ICommand
{
    private readonly int _totalSteps;
    private int _currentStep;
    private readonly object _lock = new();

    public LongRunningCommand(int totalSteps)
    {
        _totalSteps = totalSteps;
        _currentStep = 0;
    }

    public void Execute()
    {
        lock (_lock)
        {
            if (_currentStep < _totalSteps)
            {
                Thread.Sleep(10);
                _currentStep++;
            }
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
}
