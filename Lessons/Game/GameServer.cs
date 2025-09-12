using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using Autofac;
using Lessons.Adapters;
using Lessons.Commands;
using Lessons.Dto;
using Lessons.Infrastructure;
using MassTransit;

namespace Lessons.Game;

public class GameServer : IConsumer<GameCommand>, IConsumer<GameEndedEvent>
{
    private readonly ConcurrentDictionary<Guid, Game> _games = new ConcurrentDictionary<Guid, Game>();
    public IBusControl BusControl { get; set; }

    public Task Consume(ConsumeContext<GameCommand> context)
    {
        var gameId = context.Message.GameId;
        if (_games.TryGetValue(gameId, out var game))
        {
            game.ExecuteCommand(context.Message);
        }

        return Task.CompletedTask;
    }

    public bool InProgress(Guid id)
    {
        return _games.ContainsKey(id);
    }

    public Guid StartNewGame()
    {
        var gameId = Guid.NewGuid();
        var newGame = new Game(RegisterRules, BusControl) { Id = gameId };
        newGame.Start();
        _games.TryAdd(gameId, newGame);

        return gameId;
    }

    // Регистрируем специфицные правли и команды для каждой игры
    private void RegisterRules(ContainerBuilder registerCommand, BlockingCollection<ICommand> queue, Game game)
    {
        registerCommand.RegisterInstance<BlockingCollection<ICommand>>(queue);
        registerCommand.RegisterInstance<IStoppable>(game);
        registerCommand.RegisterType<HardStopCommand>().Named<ICommand>(nameof(HardStopCommand));
        registerCommand.RegisterType<MovingCommand>().Named<ICommand>(nameof(MovingCommand));
        registerCommand.RegisterType<AddObjectCommand>().Named<ICommand>(nameof(AddObjectCommand));
        registerCommand.RegisterType<MovableAdapter>().As<IMovableObject>();
    }

    public Task Consume(ConsumeContext<GameEndedEvent> context)
    {
        _games.TryRemove(context.Message.Id, out _);
        return Task.CompletedTask;
    }

    public IReadOnlyDictionary<string, string>? GetGameObject(Guid gameId, Guid objectId)
    {
        if(_games.TryGetValue(gameId, out var game))
        {
            if (game.GameObjects.TryGetValue(objectId, out var value))
            {
                return new ReadOnlyDictionary<string, string>(value.ToDictionary(i => i.Key, i => i.Value.ToString()));
            }
        }
        
        return null;
    }
}