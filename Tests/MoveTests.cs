using Lessons;
using Lessons.Adapters;
using Lessons.Exceptions;
using Lessons.Helpers;
using Xunit;

namespace Tests;

public class MoveTests
{
    [Fact]
    public void DirectMove()
    {
        // Arrange
        var universalObject = new UObject();
        var velocityChangeableAdapter = new VelocityChangeableAdapter(universalObject)
        {
            Velocity = new Vector()
            {
                X = -7, Y = 3
            }
        };
        var movableAdapter = new MovableAdapter(universalObject);
        movableAdapter.Location = new Vector()
        {
            X = 12, Y = 5
        };
        // Act
        movableAdapter.Move();

        // Assert
        Assert.Equal(movableAdapter.Location.X, 5);
        Assert.Equal(movableAdapter.Location.Y, 8);
    }

    [Fact]
    public void MoveWithoutLocation()
    {
        // Arrange
        var universalObject = new UObject();
        var velocityChangeableAdapter = new VelocityChangeableAdapter(universalObject)
        {
            Velocity = new Vector()
            {
                X = -7, Y = 3
            }
        };
        var movableAdapter = new MovableAdapter(universalObject);
        // Act
        Assert.Throws<NotMovableObjectException>(() => movableAdapter.Move());
    }

    [Fact]
    public void MoveWithoutVelocity()
    {
        // Arrange
        var universalObject = new UObject();

        var movableAdapter = new MovableAdapter(universalObject);
        movableAdapter.Location = new Vector()
        {
            X = 12, Y = 5
        };
        // Act
        Assert.Throws<NotMovableObjectException>(() => movableAdapter.Move());
    }

    [Fact]
    public void Rotate()
    {
        // Arrange
        var universalObject = new UObject();
        var rotatableAdapter = new RotatableAdapter(universalObject)
        {
            Angular = 45
        };
        var angularVelocityAdapter = new AngularVelocityChangeableAdapter(universalObject)
        {
            AngularVelocity = 15
        };
        // Act
        rotatableAdapter.Rotate();

        // Assert
        Assert.Equal(rotatableAdapter.Angular, 60);
    }
    
    [Fact]
    public void RotateWithoutVelocity()
    {
        // Arrange
        var universalObject = new UObject();

        var movableAdapter = new RotatableAdapter(universalObject);
        // Act
        Assert.Throws<NotRotatableObjectException>(() => movableAdapter.Rotate());
    }
}