namespace Lesson4;

public static class MovingHelper
{
    public static void Move(this IMovableObject movableObject)
    {
        try
        {
            var newLocation = new Vector()
            {
                X = movableObject.Location.X + movableObject.Velocity.X,
                Y = movableObject.Location.Y + movableObject.Velocity.Y,
            };
            movableObject.Location = newLocation;
        }
        catch (KeyNotFoundException e)
        {
            throw new NotMovableObjectException();
        }
    }
}