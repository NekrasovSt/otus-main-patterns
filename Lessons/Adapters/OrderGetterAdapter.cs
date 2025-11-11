namespace Lessons.Adapters;

public class OrderGetterAdapter : IOrderGetter
{
    private readonly UObject _uObject;

    public OrderGetterAdapter(UObject uObject)
    {
        _uObject = uObject;
    }

    public Guid Id => (Guid)_uObject[nameof(Id)];

    public Guid Player => (Guid)_uObject[nameof(Player)];

    public string Command => (string)_uObject[nameof(Command)];

    public Dictionary<string, object>? Args => _uObject.ContainsKey(nameof(Args))
        ? (Dictionary<string, object>)_uObject[nameof(Args)]
        : null;
}