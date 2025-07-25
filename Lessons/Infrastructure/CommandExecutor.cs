using System.Collections.Concurrent;

namespace Lesson4.Infrastructure;

public class CommandExecutor
{
    private readonly BlockingCollection<ICommand> _blockingCollection;
    private readonly ExecutionHandler _executionHandler;

    public CommandExecutor(BlockingCollection<ICommand> blockingCollection, ExecutionHandler executionHandler)
    {
        _blockingCollection = blockingCollection;
        _executionHandler = executionHandler;
    }

    public void ExecuteSingleCommand()
    {
        if (_blockingCollection.TryTake(out var command))
        {
            try
            {
                command.Execute();
            }
            catch (Exception e)
            {
                _executionHandler.Handle(command, e)?.Execute();
            }
        }
    }
}