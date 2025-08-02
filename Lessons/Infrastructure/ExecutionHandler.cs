using System.Collections.Concurrent;
using Lessons.Commands;
using Lessons.Exceptions;

namespace Lessons.Infrastructure;

public class ExecutionHandler
{
    private readonly BlockingCollection<ICommand> _blockingCollection;
    private readonly ILogger _logger;

    public ExecutionHandler(BlockingCollection<ICommand> blockingCollection, ILogger logger)
    {
        _blockingCollection = blockingCollection ?? throw new ArgumentNullException(nameof(blockingCollection));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public ICommand? Handle(ICommand command, Exception e)
    {
        return command switch
        {
            RotateCommand when e is NotRotatableObjectException => new EnqueueCommand(_blockingCollection,
                new RetryCommand(command, 0)),
            RetryCommand when e is NotRotatableObjectException => new WriteLogCommand(_logger, e),
            MovingCommand when e is NotMovableObjectException => new EnqueueCommand(_blockingCollection, 
                new RetryCommand(command, 0)),
            RetryCommand cmd when e is NotMovableObjectException && cmd.NumberOfRetries == 0 => new EnqueueCommand(_blockingCollection, 
                new RetryCommand(command, 1)),
            RetryCommand cmd when e is NotMovableObjectException && cmd.NumberOfRetries >= 1  => new WriteLogCommand(_logger, e),
            _ => null
        };
    }
}