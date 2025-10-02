using Lessons.Infrastructure;

namespace Lessons.Commands;

public class RunCommand: ICommand
{
    private readonly StateServer _stateServer;

    public RunCommand(StateServer stateServer)
    {
        _stateServer = stateServer;
    }

    public void Execute()
    {
        _stateServer.Normal();
    }
}