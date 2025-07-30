namespace Lessons.Infrastructure;

public class MacroCommand: ICommand
{
    private readonly IEnumerable<ICommand> _commands;

    public MacroCommand(IEnumerable<ICommand> commands)
    {
        _commands = commands;
    }

    public void Execute()
    {
        foreach (var command in _commands)
        {
            command.Execute();
        }
    }
}