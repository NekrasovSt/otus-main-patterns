using System.Collections.Concurrent;
using Lessons.Infrastructure;

namespace Lessons.Commands;

public class RegisterCommand: ICommand
{
    private readonly string _name;
    private readonly Func<object[], object> _createFunc;
    private readonly ConcurrentDictionary<string, Func<object[], object>> _scope;

    public RegisterCommand(string name, Func<object[], object> createFunc, ConcurrentDictionary<string, Func<object[], object>> scope)
    {
        _name = name;
        _createFunc = createFunc;
        _scope = scope;
    }

    public void Execute()
    {
        _scope.TryAdd(_name, _createFunc);
    }
}