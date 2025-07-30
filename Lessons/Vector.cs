namespace Lesson4;

public class Vector
{
    public int X { get; set; }
    public int Y { get; set; }

    public int Length => (int)Math.Round(Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2)));

    public override string ToString()
    {
        return $"({X}, {Y}) L={Length}";
    }
}