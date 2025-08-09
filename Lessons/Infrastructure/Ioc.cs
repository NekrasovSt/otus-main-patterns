using System.Collections.Concurrent;
using Lessons.Commands;
using Lessons.Exceptions;

namespace Lessons.Infrastructure;

public class Ioc
{
    private static ThreadLocal<ConcurrentDictionary<string, Func<object[], object>> > _currentScopes = new(true);
    private static readonly ConcurrentDictionary<string, Func<object[], object>> _rootScope = new();

    static Ioc()
    {
        _rootScope.TryAdd("IoC.NewScope", (object[] args) => new NewScopeCommand(_currentScopes, _currentScopes.Value ?? _rootScope));
        _rootScope.TryAdd("IoC.Register", (object[] args) => new RegisterCommand((string)args[0], (Func<object[], object>)args[1], _currentScopes.Value ?? _rootScope));
        _rootScope.TryAdd("IoC.Clear", (object[] args) => new ClearCommand(_currentScopes));
    }
    public static T Resolve<T>(string name, params object[] args) where T: class
    {
        var currentScope = _currentScopes.Value ?? _rootScope;
        if (currentScope.TryGetValue(name, out var func))
        {
            return func(args) as T;
        }

        while (currentScope.TryGetValue("IoC.ParentScope", out var parent))
        {
            currentScope = parent([]) as ConcurrentDictionary<string, Func<object[], object>>;
            if (currentScope != null && currentScope.TryGetValue(name, out func))
            {
                return func(args) as T;
            }
        }

        throw new DependencyNotFoundException();
    }
}