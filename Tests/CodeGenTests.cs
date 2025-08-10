using Lessons;
using Lessons.Commands;
using Lessons.Helpers;
using Lessons.Infrastructure;
using Moq;
using Xunit;

namespace Tests;

public class CodeGenTests
{
    [Fact]
    public void CreateInstance()
    {
        // Arrange
        var universalObject = new UObject();

        // Act
        var adapter = ReflectionGenerator.CreateInstance<IMovableObject>(universalObject);

        // Assert
        Assert.NotNull(adapter);
        Assert.IsAssignableFrom<IMovableObject>(adapter);
    }

    [Fact]
    public void GetVelocity()
    {
        // Arrange
        var universalObject = StarshipBuilder
            .CreateObject()
            .SetVelocity(0, 3);
        var adapter = ReflectionGenerator.CreateInstance<IMovableObject>(universalObject);
        Ioc.Resolve<ICommand>("IoC.NewScope").Execute();
        Ioc.Resolve<ICommand>("IoC.Register", "Spaceship.Operations.IMovableObject:Velocity.get",
            (object[] args) => ((UObject)args[0])[nameof(IMovableObject.Velocity)]).Execute();
        // Act
        var velocity = adapter.Velocity;
        
        // Assert
        Assert.Equal(new Vector() { X = 0, Y = 3 }, velocity);
    }
    
    [Fact]
    public void GetLocation()
    {
        // Arrange
        var universalObject = StarshipBuilder
            .CreateObject()
            .SetLocation(0, 3);
        var adapter = ReflectionGenerator.CreateInstance<IMovableObject>(universalObject);
        Ioc.Resolve<ICommand>("IoC.NewScope").Execute();
        Ioc.Resolve<ICommand>("IoC.Register", "Spaceship.Operations.IMovableObject:Location.get",
            (object[] args) => ((UObject)args[0])[nameof(IMovableObject.Location)]).Execute();
        
        // Act
        var velocity = adapter.Location;
        
        // Assert
        Assert.Equal(new Vector() { X = 0, Y = 3 }, velocity);
    }
    
    [Fact]
    public void GetLocationAutoRegisterDeps()
    {
        // Arrange
        var universalObject = StarshipBuilder
            .CreateObject()
            .SetLocation(0, 3);
        var factory = ReflectionGenerator.CreateFactory<IMovableObject>();
        Ioc.Resolve<ICommand>("IoC.NewScope").Execute();
        factory.RegisterDependencies();
        var adapter = factory.Create(universalObject) as IMovableObject;

        
        // Act
        var velocity = adapter.Location;
        
        // Assert
        Assert.Equal(new Vector() { X = 0, Y = 3 }, velocity);
    }
    
    [Fact]
    public void SetLocation()
    {
        // Arrange
        var universalObject = StarshipBuilder
            .CreateObject();
        
        var adapter = ReflectionGenerator.CreateInstance<IMovableObject>(universalObject);
        Ioc.Resolve<ICommand>("IoC.NewScope").Execute();
        Ioc.Resolve<ICommand>("IoC.Register", "Spaceship.Operations.IMovableObject:Location.set",
            (object[] args) => new LocationSetCommand((UObject)args[0], (Vector)args[1])).Execute();
        
        // Act
        adapter.Location = new Vector() { X = 0, Y = 3 };
        
        // Assert
        Assert.Equal(new Vector() { X = 0, Y = 3 }, universalObject["Location"]);
    }

    [Fact]
    public void CreateInstanceWithMethod()
    {
        // Arrange
        var universalObject = new UObject();

        // Act
        var adapter = ReflectionGenerator.CreateInstance<IMovableExt>(universalObject);
        
        // Assert
        Assert.NotNull(adapter);
        Assert.IsAssignableFrom<IMovableExt>(adapter);
    }

    [Fact]
    public void InvokeMethod()
    {
        // Arrange
        var universalObject = new UObject();
        var adapter = ReflectionGenerator.CreateInstance<IMovableExt>(universalObject);
        var command = new Mock<ICommand>();
        Ioc.Resolve<ICommand>("IoC.Register", "Spaceship.Operations.IMovableExt:method.Finish",
            (object[] args) => command.Object).Execute();
        // Act
        adapter.Finish();
        
        // Assert
        command.Verify(i=>i.Execute());
    }
}