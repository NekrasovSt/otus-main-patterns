using System.Collections.Concurrent;
using Autofac;
using Lessons.Adapters;
using Lessons.Exceptions;
using Lessons.Infrastructure;
using Lessons.Orders;

namespace Lessons.Commands;

public class OrderExecuteCommand: ICommand
{
    private readonly UObject _order;
    private readonly ConcurrentDictionary<Guid, UObject> _gameObjects;
    private readonly IContainer _container;

    public OrderExecuteCommand(UObject order, ConcurrentDictionary<Guid, UObject> gameObjects, IContainer container)
    {
        _order = order;
        _gameObjects = gameObjects;
        _container = container;
    }

    public void Execute()
    {
        var adapter = new OrderGetterAdapter(_order);
        if (_gameObjects.TryGetValue(adapter.Id, out var gameObject))
        {
            var playerAdapter = new PlayerGetterAdapter(gameObject);
            if (playerAdapter.PlayerId != adapter.Player)
            {
                throw new GameObjectAccessException();
            }
            _container.ResolveNamed<IOrderExecutor>(adapter.Command).Execute(gameObject, adapter.Args);
        }
        else
        {
            throw new GameObjectNotFoundException();
        }
    }
}