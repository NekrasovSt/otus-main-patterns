using DefaultNamespace;

namespace Lesson4;

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