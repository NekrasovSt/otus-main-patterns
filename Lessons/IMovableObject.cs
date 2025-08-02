namespace Lessons;

public interface IMovableObject
{
    Vector Velocity { get; }
    Vector Location { get; set; }
}