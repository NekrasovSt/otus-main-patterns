using Lessons.Infrastructure;

namespace Lessons.Commands;

public class MoveToCommand: ICommand
{
    private readonly StateServer _stateServer;

    public MoveToCommand(StateServer stateServer)
    {
        _stateServer = stateServer;
    }

    public void Execute()
    {
        _stateServer.MoveTo();
    }
}