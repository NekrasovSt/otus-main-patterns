using DefaultNamespace;

namespace Lesson4;

public class MovableAdapter : IMovableObject
{
    private readonly UObject _universalObject;

    public MovableAdapter(UObject universalObject)
    {
        _universalObject = universalObject;
    }

    public Vector Velocity
    {
        get
        {
            var velocity = (double)_universalObject[nameof(Velocity)];
            var angular = (int)_universalObject["Angular"];
            return new Vector()
            {
                X = (int)(Math.Round(velocity * Math.Cos((Math.PI / 180) * angular), MidpointRounding.ToEven)),
                Y = (int)(Math.Round(velocity * Math.Sin((Math.PI / 180) * angular), MidpointRounding.ToEven)),
            };
        }
    }

    public Vector Location
    {
        get => (Vector)_universalObject[nameof(Location)];
        set => _universalObject[nameof(Location)] = value;
    }
}