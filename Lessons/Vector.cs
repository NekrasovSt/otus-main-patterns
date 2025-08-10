namespace Lessons;

public class Vector
{
    public int X { get; set; }
    public int Y { get; set; }

    public int Length => (int)Math.Round(Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2)));

    protected bool Equals(Vector other)
    {
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Vector)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public override string ToString()
    {
        return $"({X}, {Y}) L={Length}";
    }
}