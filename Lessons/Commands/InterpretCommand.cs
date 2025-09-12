using System.Collections.Concurrent;
using Autofac;
using Lessons.Dto;
using Lessons.Infrastructure;
using MassTransit;

namespace Lessons.Commands;

public class InterpretCommand: ICommand
{
    private readonly string _commandName;
    private readonly BlockingCollection<ICommand> _blockingCollection;
    private readonly Guid _objectId;
    private readonly IContainer _container;
    private readonly IBusControl _busControl;
    public Dictionary<string, object> Args { get; init; }

    public InterpretCommand(string commandName, BlockingCollection<ICommand> blockingCollection, Guid objectId, IContainer container)
    {
        _commandName = commandName;
        _blockingCollection = blockingCollection;
        _objectId = objectId;
        _container = container;
        _busControl = _container.Resolve<IBusControl>();
    }

    public void Execute()
    {
        var game = _container.Resolve<Game.Game>();
        using var childScope = _container.BeginLifetimeScope(b =>
        {
            b.Register(ctx => Args);
            if(game.GameObjects.TryGetValue(_objectId, out var uObject))
            {
                b.Register(ctx => uObject);
            }
        });
        childScope.ResolveNamed<ICommand>(_commandName).Execute();
        _busControl.Publish(new CommandExecutedEvent() { GameId = game.Id, CommandName = _commandName});
    }
}