namespace task18;

public class SimpleCommand : ICommand
{
    private readonly Action _action;

    public SimpleCommand(Action action)
    {
        _action = action ?? throw new ArgumentNullException(nameof(action));
    }

    public void Execute() => _action();
}
