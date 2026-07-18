namespace task19;

public class HardStopCommand : ICommand
{
    public ServerThread TargetThread { get; }

    public HardStopCommand(ServerThread targetThread)
    {
        TargetThread = targetThread;
    }

    public void Execute()
    {
    }
}

