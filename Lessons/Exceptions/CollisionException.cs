namespace Lessons.Exceptions;

public class CollisionException : Exception
{
    public IEnumerable<Collision> _collisions;

    public CollisionException(IEnumerable<Collision> collisions)
    {
        _collisions = collisions;
    }
}