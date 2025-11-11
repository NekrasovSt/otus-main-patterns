namespace Lessons;

public interface IOrderGetter
{
    Guid Id { get; }
    Guid Player { get; }
    string Command { get; }
    Dictionary<string, object>? Args { get; }
}