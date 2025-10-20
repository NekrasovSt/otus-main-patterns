namespace Lessons.Orders;

public interface IOrderExecutor
{
    void Execute(UObject uObject, Dictionary<string, object>? args);
}