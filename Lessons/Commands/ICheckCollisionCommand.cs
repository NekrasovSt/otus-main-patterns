using Lessons.Exceptions;
using Lessons.Game;
using Lessons.Infrastructure;

namespace Lessons.Commands;

public class CheckCollisionCommand(GameBoard gameBoard) : ICommand
{
    public void Execute()
    {
        var collision = gameBoard.Exam();
        if (collision.Any())
        {
            throw new CollisionException(collision);
        }
    }
}