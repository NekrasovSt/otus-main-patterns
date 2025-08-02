using Lessons.Infrastructure;

namespace Lessons.Commands;

public class RetryCommand: ICommand
{
    private readonly ICommand _targetCommand;
    private readonly int _numberOfRetries;

    public RetryCommand(ICommand targetCommand, int numberOfRetries)
    {
        _targetCommand = targetCommand ?? throw new ArgumentNullException(nameof(targetCommand));
        if (numberOfRetries < 0)
        {
            throw new ArgumentException(nameof(numberOfRetries));
        }
        _numberOfRetries = numberOfRetries;
    }

    public int NumberOfRetries => _numberOfRetries;

    public void Execute()
    {
        _targetCommand.Execute();
    }
}