using System.Collections.Concurrent;
using Lessons.Infrastructure;

namespace Lessons.Commands;

public class ClearCommand : ICommand
{
    private readonly ThreadLocal<ConcurrentDictionary<string, Func<object[], object>>> _currentScope;

    public ClearCommand(ThreadLocal<ConcurrentDictionary<string, Func<object[], object>>> currentScope)
    {
        _currentScope = currentScope;
    }

    public void Execute()
    {
        _currentScope.Value = null;
    }
}