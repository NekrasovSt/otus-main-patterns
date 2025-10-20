namespace Lessons;

public interface IOrderSetter
{
    Guid Id { set; }
    Guid Player { set; }
    string Command { set; }
    Dictionary<string, object> Args { set; }
}