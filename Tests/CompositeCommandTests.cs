using Lessons;
using Lessons.Exceptions;
using Lessons.Helpers;
using Lessons.Infrastructure;
using Xunit;

namespace Tests;

public class CompositeCommandTests
{
    [Fact]
    public void CheckFuelNotEnough()
    {
        // Arrange
        var universalObject = StarshipBuilder
            .CreateObject()
            .SetVelocity(0, 3)
            .SetFuelConsumption(1)
            .SetFuelAmount(2);

        var command = new CheckFuelCommand(new FuelConsumptionAdapter(universalObject), new MovableAdapter(universalObject));
        
        // Act
        Assert.Throws<CommandException>(() => command.Execute());
    }
    
    [Fact]
    public void CheckFuelEnough()
    {
        // Arrange
        var universalObject = StarshipBuilder
            .CreateObject()
            .SetVelocity(0, 3)
            .SetFuelConsumption(1)
            .SetFuelAmount(20);

        var command = new CheckFuelCommand(new FuelConsumptionAdapter(universalObject), new MovableAdapter(universalObject));
        
        // Act
        command.Execute();
    }

    [Fact]
    public void BurnFuel()
    {
        // Arrange
        var universalObject = StarshipBuilder
            .CreateObject()
            .SetVelocity(0, 3)
            .SetFuelConsumption(1)
            .SetFuelAmount(20);
       
        var fuelConsumptionAdapter = new FuelConsumptionAdapter(universalObject);
        var command = new BurnFuelCommand(fuelConsumptionAdapter, new MovableAdapter(universalObject));
        
        // Act
        command.Execute();
        
        // Assert
        Assert.Equal(17, fuelConsumptionAdapter.FuelAmount);
    }

    [Fact]
    public void MacroCommandChainNotEnoughFuel()
    {
        // Arrange
        var universalObject = StarshipBuilder
            .CreateObject()
            .SetVelocity(0, 3)
            .SetFuelConsumption(1)
            .SetFuelAmount(2);

        var fuelConsumptionAdapter = new FuelConsumptionAdapter(universalObject);
        var checkCommand = new CheckFuelCommand(fuelConsumptionAdapter, new MovableAdapter(universalObject));
        var burnFuelCommand = new BurnFuelCommand(fuelConsumptionAdapter, new MovableAdapter(universalObject));
        var macro = new MacroCommand([checkCommand, burnFuelCommand]);
        
        //Act
        Assert.Throws<CommandException>(() => macro.Execute());
        
        // Assert
        Assert.Equal(2, fuelConsumptionAdapter.FuelAmount);
    }

    [Fact]
    public void MacroCommandChainDirectMove()
    {
        // Arrange
        var universalObject = StarshipBuilder
            .CreateObject()
            .SetVelocity(3, 0)
            .SetLocation(0, 0)
            .SetFuelConsumption(1)
            .SetFuelAmount(20);
        
        var fuelConsumptionAdapter = new FuelConsumptionAdapter(universalObject);
        var movableAdapter = new MovableAdapter(universalObject);
        var checkCommand = new CheckFuelCommand(fuelConsumptionAdapter, movableAdapter);
        var burnFuelCommand = new BurnFuelCommand(fuelConsumptionAdapter, movableAdapter);
        var moveCommand = new MovingCommand(movableAdapter);
        var macro = new MacroCommand([checkCommand, burnFuelCommand, moveCommand]);
        
        //Act
        macro.Execute();
        
        //Assert
        Assert.Equal(17, fuelConsumptionAdapter.FuelAmount);
        Assert.Equal(3, movableAdapter.Location.X);
        Assert.Equal(0, movableAdapter.Location.Y);
    }

    [Fact]
    public void ChangeVectorNotMovingObject()
    {
        // Arrange
        var universalObject = StarshipBuilder
            .CreateObject()
            .SetRotation(0, 45);

        var rotatableAdapter = new RotatableAdapter(universalObject);
        var rotateVelocityVectorCommand = new RotateVelocityVectorCommand(rotatableAdapter, new VelocityChangeableAdapter(universalObject),
            new MovableAdapter(universalObject));
        
        //Act
        Assert.Throws<NotMovableObjectException>(() => rotateVelocityVectorCommand.Execute());
    }
    
    [Fact]
    public void ChangeVector()
    {
        // Arrange
        var universalObject = StarshipBuilder
            .CreateObject()
            .SetVelocity(3, 0)
            .SetRotation(90, 0);

        var rotatableAdapter = new RotatableAdapter(universalObject);
        var movableAdapter = new MovableAdapter(universalObject);
        var rotateVelocityVectorCommand = new RotateVelocityVectorCommand(rotatableAdapter, new VelocityChangeableAdapter(universalObject),
            movableAdapter);
        
        //Act
        rotateVelocityVectorCommand.Execute();
        
        //Assert
        Assert.Equal(0, movableAdapter.Velocity.X);
        Assert.Equal(3, movableAdapter.Velocity.Y);
    }

    [Fact]
    public void MacroCommandChainRotateAndChangeVectorNotMovingObject()
    {
        // Arrange
        var universalObject = StarshipBuilder
            .CreateObject()
            .SetRotation(0, 90);

        var rotatableAdapter = new RotatableAdapter(universalObject);
        var rotateCommand = new RotateCommand(rotatableAdapter);
        var rotateVelocityVectorCommand = new RotateVelocityVectorCommand(rotatableAdapter,
            new VelocityChangeableAdapter(universalObject), new MovableAdapter(universalObject));
        var macroCommand = new MacroCommand([rotateCommand, rotateVelocityVectorCommand]);
        
        // Act
        Assert.Throws<NotMovableObjectException>(() => macroCommand.Execute());
        
        // Assert
        Assert.Equal(90, rotatableAdapter.Angular);
    }
    
    [Fact]
    public void MacroCommandChainRotateAndChangeVector()
    {
        // Arrange
        var universalObject = StarshipBuilder
            .CreateObject()
            .SetVelocity(3, 0)
            .SetRotation(0, 90);

        var rotatableAdapter = new RotatableAdapter(universalObject);
        var rotateCommand = new RotateCommand(rotatableAdapter);
        var movableAdapter = new MovableAdapter(universalObject);
        var rotateVelocityVectorCommand = new RotateVelocityVectorCommand(rotatableAdapter,
            new VelocityChangeableAdapter(universalObject), movableAdapter);
        var macroCommand = new MacroCommand([rotateCommand, rotateVelocityVectorCommand]);
        
        // Act
        macroCommand.Execute();
        
        // Assert
        Assert.Equal(90, rotatableAdapter.Angular);
        Assert.Equal(0, movableAdapter.Velocity.X);
        Assert.Equal(3, movableAdapter.Velocity.Y);
    }
}