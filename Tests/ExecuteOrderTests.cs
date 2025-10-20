using System.Collections.Concurrent;
using Autofac;
using Lessons;
using Lessons.Adapters;
using Lessons.Commands;
using Lessons.Exceptions;
using Lessons.Helpers;
using Lessons.Orders;
using Xunit;

namespace Tests;

public class ExecuteOrderTests
{
    private IContainer InitContainer()
    {
        var builder = new ContainerBuilder();

        builder.RegisterType<SetVelocityExecutor>().Named<IOrderExecutor>("SetVelocity");
        builder.RegisterType<StopMovingExecutor>().Named<IOrderExecutor>("StopMoving");
        return builder.Build();
    }

    [Fact]
    public void TryChangeNotOwnedObject()
    {
        // Arrange
        var user1 = Guid.NewGuid();
        var user2 = Guid.NewGuid();
        var dict = new ConcurrentDictionary<Guid, UObject>();

        var objectId1 = Guid.NewGuid();
        var starship1 = StarshipBuilder
            .CreateObject()
            .SetId(objectId1)
            .SetPlayerId(user1);

        var objectId2 = Guid.NewGuid();
        var starship2 = StarshipBuilder
            .CreateObject()
            .SetId(objectId2)
            .SetPlayerId(user2);
        dict.TryAdd(objectId1, starship1);
        dict.TryAdd(objectId2, starship2);

        // Act
        var order = OrderBuilder.CreateObject(objectId1, user2, "SetVelocity");
        var command = new OrderExecuteCommand(order, dict, InitContainer());
        Assert.Throws<GameObjectAccessException>(() => command.Execute());
    }

    [Fact]
    public void NotFoundObject()
    {
        // Arrange
        var user1 = Guid.NewGuid();
        var user2 = Guid.NewGuid();
        var dict = new ConcurrentDictionary<Guid, UObject>();

        var objectId1 = Guid.NewGuid();
        var starship1 = StarshipBuilder
            .CreateObject()
            .SetId(objectId1)
            .SetPlayerId(user1);

        var objectId2 = Guid.NewGuid();
        var starship2 = StarshipBuilder
            .CreateObject()
            .SetId(objectId2)
            .SetPlayerId(user2);
        dict.TryAdd(objectId1, starship1);
        dict.TryAdd(objectId2, starship2);

        // Act
        var order = OrderBuilder.CreateObject(Guid.NewGuid(), user2, "SetVelocity");
        var command = new OrderExecuteCommand(order, dict, InitContainer());
        Assert.Throws<GameObjectNotFoundException>(() => command.Execute());
    }

    [Fact]
    public void StartMove()
    {
        // Arrange
        var user1 = Guid.NewGuid();
        var objectId1 = Guid.NewGuid();
        var dict = new ConcurrentDictionary<Guid, UObject>();
        var starship1 = StarshipBuilder
            .CreateObject()
            .SetId(objectId1)
            .SetPlayerId(user1);
        dict.TryAdd(objectId1, starship1);

        // Act
        var order = OrderBuilder
            .CreateObject(objectId1, user1, "SetVelocity")
            .AddArgs(new Dictionary<string, object>()
            {
                { "X", 1 },
                { "Y", 1 },
            });
        var command = new OrderExecuteCommand(order, dict, InitContainer());
        command.Execute();

        // Assert

        var adapter = new MovableAdapter(starship1);
        Assert.Equal(new Vector() { X = 1, Y = 1 }, adapter.Velocity);
    }
    
    [Fact]
    public void StopMove()
    {
        // Arrange
        var user1 = Guid.NewGuid();
        var objectId1 = Guid.NewGuid();
        var dict = new ConcurrentDictionary<Guid, UObject>();
        var starship1 = StarshipBuilder
            .CreateObject()
            .SetId(objectId1)
            .SetPlayerId(user1);
        dict.TryAdd(objectId1, starship1);

        // Act
        var order = OrderBuilder
            .CreateObject(objectId1, user1, "StopMoving");
        var command = new OrderExecuteCommand(order, dict, InitContainer());
        command.Execute();

        // Assert

        var adapter = new MovableAdapter(starship1);
        Assert.Equal(new Vector() { X = 0, Y = 0 }, adapter.Velocity);
    }
}