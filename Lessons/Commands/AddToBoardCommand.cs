using Lessons.Game;
using Lessons.Infrastructure;

namespace Lessons.Commands;

public class AddToBoardCommand : ICommand
{
    private readonly GameBoard _gameBoard;
    private readonly UObject _uObject;

    public AddToBoardCommand(GameBoard gameBoard, UObject uObject)
    {
        _gameBoard = gameBoard;
        _uObject = uObject;
    }

    public void Execute()
    {
        _gameBoard.AddOrUpdateToBoard(_uObject);
    }
}