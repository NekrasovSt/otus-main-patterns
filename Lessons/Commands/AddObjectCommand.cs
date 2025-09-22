using System.Collections.Concurrent;
using Lessons.Adapters;
using Lessons.Dto;
using Lessons.Helpers;
using Lessons.Infrastructure;
using MassTransit;

namespace Lessons.Commands;

public class AddObjectCommand : ICommand
{
    private readonly Game.Game _game;
    private readonly UObject _uObject;
    private readonly IBusControl _busControl;

    public AddObjectCommand(Game.Game game, Dictionary<string, object> arg, IBusControl busControl)
    {
        _game = game;
        _uObject = StarshipBuilder.GetFromDictionary(arg);
        _busControl = busControl;
    }

    public void Execute()
    {
        Guid objectId;
        var adapter = new IdAdapter(_uObject);
        if (adapter.Id != Guid.Empty)
        {
            objectId = adapter.Id;
        }
        else
        {
            objectId = Guid.NewGuid();
            _uObject["Id"] = objectId;
        }

        _game.GameObjects.Add(objectId, _uObject);
        _busControl.Publish(new ObjectAddedEvent { ObjectId = objectId, GameId = _game.Id });
    }
}