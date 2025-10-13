using Lessons;
using Lessons.Adapters;
using Lessons.Commands;
using Lessons.Exceptions;
using Lessons.Game;
using Lessons.Helpers;
using Lessons.Infrastructure;
using Xunit;

namespace Tests;

public class GameBoardTests
{
    [Fact]
    public void AddObjectToBoard()
    {
        // Arrange
        var board = new GameBoard();
        var obj = StarshipBuilder
            .CreateObject()
            .SetLocation(3, 3);
        
        // Act
        board.AddOrUpdateToBoard(obj);
        
        // Arrange
        var adapter = new GameBoardPositionAdapter(obj);
        Assert.Single(adapter.Positions);
        Assert.Equal(new Position(0, 0), adapter.Positions[0]);
    }
    
    [Fact]
    public void AddObjectToBoardRightBottom()
    {
        // Arrange
        var board = new GameBoard();
        var obj = StarshipBuilder
            .CreateObject()
            .SetLocation(99, 99);
        
        // Act
        board.AddOrUpdateToBoard(obj);
        
        // Arrange
        var adapter = new GameBoardPositionAdapter(obj);
        Assert.Single(adapter.Positions);
        Assert.Equal(new Position(9, 9), adapter.Positions[0]);
    }
    
    [Fact]
    public void AddObjectToBoardBetweenAreas()
    {
        // Arrange
        var board = new GameBoard();
        var obj = StarshipBuilder
            .CreateObject()
            .SetLocation(10, 3);
        
        // Act
        board.AddOrUpdateToBoard(obj);
        
        // Arrange
        var adapter = new GameBoardPositionAdapter(obj);
        Assert.Equal(2, adapter.Positions.Count);
        Assert.Equal([new Position(1, 0), new Position(0, 0)], adapter.Positions);
    }

    [Fact]
    public void MoveObject()
    {
        // Arrange
        var board = new GameBoard();
        var obj = StarshipBuilder
            .CreateObject()
            .SetLocation(3, 3);
        board.AddOrUpdateToBoard(obj);
        
        // Act
        obj.SetLocation(12, 3);
        board.AddOrUpdateToBoard(obj);
        
        // Arrange
        var adapter = new GameBoardPositionAdapter(obj);
        Assert.Single(adapter.Positions);
        Assert.Equal(new Position(1, 0), adapter.Positions[0]);
    }

    [Fact]
    public void TwoObjectsCollision()
    {
        // Arrange
        var board = new GameBoard();
        var obj1 = StarshipBuilder
            .CreateObject()
            .SetLocation(3, 3);
        board.AddOrUpdateToBoard(obj1);
        var obj2 = StarshipBuilder
            .CreateObject()
            .SetLocation(3, 3);
        board.AddOrUpdateToBoard(obj2);
        
        // Act
        var collision = board.Exam();

        // Assert
        Assert.Single(collision);
    }
    
    [Fact]
    public void NotCollision()
    {
        // Arrange
        var board = new GameBoard();
        var obj1 = StarshipBuilder
            .CreateObject()
            .SetLocation(3, 3);
        board.AddOrUpdateToBoard(obj1);
        var obj2 = StarshipBuilder
            .CreateObject()
            .SetLocation(4, 3);
        board.AddOrUpdateToBoard(obj2);
        
        // Act
        var collision = board.Exam();

        // Assert
        Assert.Empty(collision);
    }

    [Fact]
    public void MacroCommadCollision()
    {
        // Arrange
        var board = new GameBoard();
        var obj1 = StarshipBuilder
            .CreateObject()
            .SetLocation(3, 3);
        var obj2 = StarshipBuilder
            .CreateObject()
            .SetLocation(2, 2)
            .SetVelocity(1, 1);

        var command = new MacroCommand([
            new AddToBoardCommand(board, obj1), 
            new AddToBoardCommand(board, obj2), new MovingCommand(new MovableAdapter(obj2)), new AddToBoardCommand(board, obj2), new CheckCollisionCommand(board)]);

        Assert.Throws<CollisionException>(() => command.Execute());
    }
}