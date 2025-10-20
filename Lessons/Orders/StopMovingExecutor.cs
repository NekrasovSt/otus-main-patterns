using Lessons.Adapters;

namespace Lessons.Orders;

public class StopMovingExecutor : IOrderExecutor
{
    public void Execute(UObject uObject, Dictionary<string, object>? args)
    {
        var velocityChangeableAdapter = new VelocityChangeableAdapter(uObject)
        {
            Velocity = new Vector()
            {
                X = 0,
                Y = 0
            }
        };
    }
}