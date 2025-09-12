using Lessons.Infrastructure;

namespace Lessons.Commands;

public class HardStopCommand : ICommand
{
    private readonly IStoppable _serverThread;

    public HardStopCommand(IStoppable serverThread) {
        _serverThread = serverThread;
    }

    public void Execute() {
        _serverThread.Stop();
    }
}