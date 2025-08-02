namespace Lessons.Adapters;

public class VelocityChangeableAdapter : IVelocityChangeable
{
    private readonly UObject _universalObject;

    public VelocityChangeableAdapter(UObject universalObject)
    {
        _universalObject = universalObject;
    }

    public Vector Velocity
    {
        set => _universalObject[nameof(Velocity)] = value;
    }
}