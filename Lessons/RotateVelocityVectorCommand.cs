using Lessons.Infrastructure;

namespace Lessons;

public class RotateVelocityVectorCommand : ICommand
{
    private readonly IRotatableObject _rotatableObject;
    private readonly IVelocityChangeable _velocityChangeable;
    private readonly IMovableObject _movableObject;

    public RotateVelocityVectorCommand(IRotatableObject rotatableObject, IVelocityChangeable velocityChangeable,
        IMovableObject movableObject)
    {
        _rotatableObject = rotatableObject;
        _velocityChangeable = velocityChangeable;
        _movableObject = movableObject;
    }

    public void Execute()
    {
        try
        {
            var currentLocation = _movableObject.Velocity;
            if (currentLocation == null)
            {
                throw new NotMovableObjectException();
            }
        }
        catch (KeyNotFoundException e)
        {
            throw new NotMovableObjectException();
        }

        var angular = (Math.PI / 180) * _rotatableObject.Angular;
        _velocityChangeable.Velocity = new Vector()
        {
            X = (int)(Math.Round(_movableObject.Velocity.X * Math.Cos(angular) - _movableObject.Velocity.Y * Math.Sin(angular), MidpointRounding.ToEven)),
            Y = (int)(Math.Round(_movableObject.Velocity.X * Math.Sin(angular) + _movableObject.Velocity.Y * Math.Cos(angular), MidpointRounding.ToEven)),
        };
    }
}