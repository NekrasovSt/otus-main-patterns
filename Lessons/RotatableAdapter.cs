using DefaultNamespace;

namespace Lesson4;

public class RotatableAdapter : IRotatableObject
{
    private readonly UObject _universalObject;

    public RotatableAdapter(UObject universalObject)
    {
        _universalObject = universalObject;
    }

    public int Angular
    {
        get => (int)_universalObject["Angular"];
        set => _universalObject["Angular"] = value;
    }

    public int AngularVelocity => (int)_universalObject[nameof(AngularVelocity)];
}