using Lesson4.Infrastructure;

namespace Lesson4;

public class RotateCommand: ICommand
{
    private readonly IRotatableObject _rotatableObject;

    public RotateCommand(IRotatableObject rotatableObject)
    {
        _rotatableObject = rotatableObject;
    }
    
    public void Execute()
    {
        _rotatableObject.Rotate();
    }
}