namespace Lessons.Adapters;

public class OrderSetterAdapter : IOrderSetter
{
    private readonly UObject _uObject;

    public OrderSetterAdapter(UObject uObject)
    {
        _uObject = uObject;
    }

    public Guid Id
    {
        set => _uObject[nameof(Id)] = value;
    }

    public Guid Player
    {
        set => _uObject[nameof(Player)] = value;
    }

    public string Command
    {
        set => _uObject[nameof(Command)] = value;
    }

    public Dictionary<string, object> Args
    {
        set => _uObject[nameof(Args)] = value;
    }
}