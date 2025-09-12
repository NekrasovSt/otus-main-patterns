using Lessons.Game;
using MassTransit;

namespace Lessons.Helpers;

public class GameHelper
{
    public static async Task<(IBusControl, GameServer)> InitGame(IEnumerable<object> consumers)
    {
        var game = new GameServer();
        var bus = Bus.Factory.CreateUsingInMemory(cfg=>
        {
            cfg.ReceiveEndpoint(e =>
            {
                e.Instance(game);
                foreach (var consumer in consumers)
                {
                    e.Instance(consumer);
                }
            });
        });
        game.BusControl = bus;
        await bus.StartAsync();
        return (bus, game);
    }
}