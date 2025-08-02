using Lessons.Infrastructure;

namespace Lessons.Commands;

public class WriteLogCommand: ICommand
{
    private readonly ILogger _logger;
    private readonly Exception _targetException;

    public WriteLogCommand(ILogger logger, Exception targetException)
    {
        _logger = logger;
        _targetException = targetException;
    }

    public void Execute()
    {
        _logger.Log(_targetException);
    }
}