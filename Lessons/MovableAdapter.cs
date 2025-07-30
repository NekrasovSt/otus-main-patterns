using DefaultNamespace;

namespace Lesson4;

public class MovableAdapter : IMovableObject
{
    private readonly UObject _universalObject;

    public MovableAdapter(UObject universalObject)
    {
        _universalObject = universalObject;
    }

    public Vector Velocity => (Vector)_universalObject[nameof(Velocity)];

    public Vector Location
    {
        get => (Vector)_universalObject[nameof(Location)];
        set => _universalObject[nameof(Location)] = value;
    }
}