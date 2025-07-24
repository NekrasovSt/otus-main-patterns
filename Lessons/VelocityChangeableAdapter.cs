using DefaultNamespace;

namespace Lesson4;

public class VelocityChangeableAdapter : IVelocityChangeable
{
    private readonly UObject _universalObject;

    public VelocityChangeableAdapter(UObject universalObject)
    {
        _universalObject = universalObject;
    }

    public Vector Velocity
    {
        set
        {
            var velocity = Math.Sqrt(Math.Pow(value.X, 2) + Math.Pow(value.Y, 2));
            _universalObject[nameof(Velocity)] = velocity;
            _universalObject["Angular"] = (int)(Math.Acos((double)value.X / velocity) * (180 / Math.PI));
        }
    }
}