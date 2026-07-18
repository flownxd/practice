namespace task19;

public class TestCommand : ICommand
{
    private readonly int _id;
    private int _counter;
    private readonly int _maxExecutions;
    private readonly IScheduler? _scheduler;

    public TestCommand(int id, int maxExecutions = 3, IScheduler? scheduler = null)
    {
        _id = id;
        _maxExecutions = maxExecutions;
        _counter = 0;
        _scheduler = scheduler;
    }

    public void Execute()
    {
        _counter++;
        Console.WriteLine($"Поток {_id} вызов {_counter}");
        
        if (_counter < _maxExecutions && _scheduler != null)
        {
            _scheduler.Add(this);
        }
    }

    public int Counter => _counter;
    public bool IsCompleted => _counter >= _maxExecutions;
    public int Id => _id;
}