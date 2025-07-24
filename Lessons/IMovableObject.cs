namespace Lesson4;

public interface IMovableObject
{
    Vector Velocity { get; }
    Vector Location { get; set; }
}