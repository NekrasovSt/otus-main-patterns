namespace Lesson4;

public class Vector
{
    public int X { get; set; }
    public int Y { get; set; }

    public override string ToString()
    {
        return "(" + X + ", " + Y + ")";
    }
}