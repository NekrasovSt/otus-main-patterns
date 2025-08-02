using Lessons.Helpers;
using Lessons.Infrastructure;

namespace Lessons.Commands;

public class MovingCommand: ICommand
{
    private readonly IMovableObject _movableObject;

    public MovingCommand(IMovableObject movableObject)
    {
        _movableObject = movableObject;
    }

    public void Execute()
    {
        _movableObject.Move();
    }
}