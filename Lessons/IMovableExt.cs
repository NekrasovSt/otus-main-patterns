namespace Lessons;

public interface IMovableExt
{
    Vector Velocity { get; }
    Vector Location { get; set; }
    void Finish();
}