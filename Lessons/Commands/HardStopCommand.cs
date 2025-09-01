using Lessons.Infrastructure;

namespace Lessons.Commands;

public class HardStopCommand : ICommand
{
    private readonly ServerThread _serverThread;

    public HardStopCommand(ServerThread serverThread) {
        _serverThread = serverThread;
    }

    public void Execute() {
        _serverThread.Stop();
    }
}