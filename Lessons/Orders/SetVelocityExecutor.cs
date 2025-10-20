using Lessons.Adapters;

namespace Lessons.Orders;

public class SetVelocityExecutor: IOrderExecutor
{
    public void Execute(UObject uObject, Dictionary<string, object> args)
    {
        var velocityChangeableAdapter = new VelocityChangeableAdapter(uObject)
        {
            Velocity = new Vector()
            {
                X = (int)args[nameof(Vector.X)],
                Y = (int)args[nameof(Vector.Y)]
            }
        };
    }
}