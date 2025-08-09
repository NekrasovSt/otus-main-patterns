using System.Collections.Concurrent;
using Lessons.Infrastructure;

namespace Lessons.Commands;

public class NewScopeCommand: ICommand
{
    private readonly ThreadLocal<ConcurrentDictionary<string, Func<object[], object>>> _currentScope;
    private readonly ConcurrentDictionary<string, Func<object[], object>> _parentScope;

    public NewScopeCommand(ThreadLocal<ConcurrentDictionary<string, Func<object[], object>>> currentScope, ConcurrentDictionary<string, Func<object[], object>> parentScope)
    {
        _currentScope = currentScope;
        _parentScope = parentScope;
    }

    public void Execute()
    {
        var newScope = new ConcurrentDictionary<string, Func<object[], object>>();
        newScope.TryAdd("IoC.ParentScope", (object[] args) => _parentScope);
        _currentScope.Value = newScope;
    }
}