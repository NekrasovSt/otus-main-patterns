namespace Lessons.Adapters;

public class PlayerSetterAdapter : IPlayerSetter
{
    private readonly UObject _uObject;

    public PlayerSetterAdapter(UObject uObject)
    {
        _uObject = uObject;
    }

    public Guid PlayerId
    {
        set => _uObject[nameof(PlayerId)] = value;
    }
}