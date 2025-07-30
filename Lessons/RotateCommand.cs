using Lessons.Infrastructure;

namespace Lessons;

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