using Lessons.Commands;
using Lessons.Dto;
using Lessons.Helpers;
using Xunit;

namespace Tests;

public class GameServerTests
{
    [Fact]
    public async Task StartGameShouldWaitEvent()
    {
        // Arrange
        var waiter = new EventWaiter<GameStartedEvent>();
        var (bus, gameServer) = await GameHelper.InitGame([waiter]);

        // Act
        var newGame = gameServer.StartNewGame();
        waiter.Wait();
        
        // Assert
        Assert.True(gameServer.InProgress(newGame));
    }
    
    [Fact]
    public async Task StopGameShouldWaitEvent()
    {
        // Arrange
        var waiter = new EventWaiter<GameEndedEvent>();
        var (bus, gameServer) = await GameHelper.InitGame([waiter]);

        // Act
        var newGame = gameServer.StartNewGame();
        await bus.Publish(new GameCommand { CommandName = nameof(HardStopCommand), GameId = newGame });
        waiter.Wait();
        
        // Assert
        Assert.False(gameServer.InProgress(newGame));
    }

    [Fact]
    public async Task AddObjectToGame()
    {
        // Arrange
        var waiter = new EventWaiter<ObjectAddedEvent>();
        var (bus, gameServer) = await GameHelper.InitGame([waiter]);
        
        // Act
        var newGame = gameServer.StartNewGame();
        var gameObject = StarshipBuilder.CreateObject().SetLocation(1, 1).SetVelocity(1, 0);
        await bus.Publish(new GameCommand() { CommandName = nameof(AddObjectCommand), GameId = newGame, Args = gameObject });
        
        waiter.Wait();
        
        // Assert
        var obj = gameServer.GetGameObject(newGame, waiter.Event.ObjectId);
        Assert.NotNull(obj);
    }

    [Fact]
    public async Task ExecuteCommand()
    {
        // Arrange
        var waiter = new EventWaiter<CommandExecutedEvent>(i=>i.CommandName == nameof(MovingCommand));
        var (bus, gameServer) = await GameHelper.InitGame([waiter]);
        var newGame = gameServer.StartNewGame();
        var objectId = Guid.NewGuid();
        var gameObject = StarshipBuilder
            .CreateObject()
            .SetId(objectId)
            .SetLocation(1, 1)
            .SetVelocity(1, 0);
        await bus.Publish(new GameCommand() { CommandName = nameof(AddObjectCommand), GameId = newGame, Args = gameObject });
        
        // Act
        await bus.Publish(new GameCommand() { CommandName = nameof(MovingCommand), GameId = newGame, Object = objectId });
        waiter.Wait();
        
        // Assert
        var uobject = gameServer.GetGameObject(newGame, objectId);
        Assert.Equal("(2, 1) L=2", uobject["Location"]);
    }
}