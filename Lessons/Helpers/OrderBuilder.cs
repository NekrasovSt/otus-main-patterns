using Lessons.Adapters;

namespace Lessons.Helpers;

public static class OrderBuilder
{
    public static UObject CreateObject(Guid id, Guid playerId, string command)
    {
        var obj = new UObject();
        var adapter = new OrderSetterAdapter(obj)
        {
            Id = id,
            Player = playerId,
            Command = command
        };
        return obj;
    }

    public static UObject AddArgs(this UObject obj, Dictionary<string, object> args)
    {
        var adapter = new OrderSetterAdapter(obj)
        {
            Args = args
        };
        return obj;
    }
}