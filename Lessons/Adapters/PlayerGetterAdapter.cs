namespace Lessons.Adapters;

public class PlayerGetterAdapter : IPlayerGetter
{
    private readonly UObject _uObject;

    public PlayerGetterAdapter(UObject uObject)
    {
        _uObject = uObject;
    }

    public Guid PlayerId => (Guid)_uObject[nameof(PlayerId)];
}