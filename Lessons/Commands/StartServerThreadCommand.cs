using Lessons.Infrastructure;

namespace Lessons.Commands;

public class StartServerThreadCommand(ServerThread serverThread) : ICommand
{
    public void Execute()
    {
        serverThread.Start();
    }
}