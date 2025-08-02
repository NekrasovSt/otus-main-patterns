using Lessons.Exceptions;

namespace Lessons.Helpers;

public static class RotateHelper
{
    public static void Rotate(this IRotatableObject rotatableObject)
    {
        try
        {
            rotatableObject.Angular += rotatableObject.AngularVelocity;
        }
        catch (KeyNotFoundException e)
        {
            throw new NotRotatableObjectException();
        }
    }
}