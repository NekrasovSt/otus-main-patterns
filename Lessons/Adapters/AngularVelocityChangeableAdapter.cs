namespace Lessons.Adapters;

public class AngularVelocityChangeableAdapter : IAngularVelocityChangeable
{
    private readonly UObject _universalObject;

    public AngularVelocityChangeableAdapter(UObject universalObject)
    {
        _universalObject = universalObject;
    }

    public int AngularVelocity
    {
        set => _universalObject[nameof(AngularVelocity)] = value;
    }
}