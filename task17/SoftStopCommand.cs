namespace task17;

public class SoftStopCommand : ICommand
{
    public ServerThread TargetThread { get; }

    public SoftStopCommand(ServerThread targetThread)
    {
        TargetThread = targetThread;
    }

    public void Execute()
    {
    }
}