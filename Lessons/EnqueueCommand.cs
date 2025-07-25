using System.Collections.Concurrent;
using Lesson4.Infrastructure;

namespace Lesson4;

public class EnqueueCommand: ICommand
{
    private readonly BlockingCollection<ICommand> _blockingCollection;
    private readonly ICommand _targetCommand;

    public EnqueueCommand(BlockingCollection<ICommand> blockingCollection, ICommand targetCommand)
    {
        _blockingCollection = blockingCollection;
        _targetCommand = targetCommand;
    }

    public void Execute()
    {
        _blockingCollection.Add(_targetCommand);
    }
}