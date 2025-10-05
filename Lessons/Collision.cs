namespace Lessons;

public class Collision(UObject firstObject, UObject secondObject)
{
    public UObject FirstObject { get; } = firstObject;
    public UObject SecondObject { get; } = secondObject;
}