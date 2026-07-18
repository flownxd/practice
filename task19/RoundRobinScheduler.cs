using System.Collections.Concurrent;

namespace task19;

public class RoundRobinScheduler : IScheduler
{
    private readonly ConcurrentQueue<ICommand> _queue = new();

    public bool HasCommand() => !_queue.IsEmpty;

    public ICommand? Select()
    {
        return _queue.TryDequeue(out var command) ? command : null;
    }

    public void Add(ICommand cmd)
    {
        if (cmd == null)
            throw new ArgumentNullException(nameof(cmd));
        _queue.Enqueue(cmd);
    }
}

