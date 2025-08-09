using Lessons;
using Lessons.Adapters;
using Lessons.Commands;
using Lessons.Exceptions;
using Lessons.Helpers;
using Lessons.Infrastructure;
using Xunit;

namespace Tests;

public class IocTests : IDisposable
{
    [Fact]
    public void NotFoundDependency()
    {
        //Arrange
        var universalObject = StarshipBuilder
            .CreateObject()
            .SetRotation(0, 0);
        var movableAdapter = new RotatableAdapter(universalObject);
        Ioc.Resolve<ICommand>("IoC.NewScope").Execute();

        // Act
        Assert.Throws<DependencyNotFoundException>(() => Ioc.Resolve<ICommand>("RotateCommand", movableAdapter));
    }

    [Fact]
    public void ResolveDependency()
    {
        //Arrange
        var universalObject = StarshipBuilder
            .CreateObject()
            .SetLocation(0, 0)
            .SetVelocity(1, 0);
        var movableAdapter = new MovableAdapter(universalObject);
        Ioc.Resolve<ICommand>("IoC.NewScope").Execute();
        Ioc.Resolve<ICommand>("IoC.Register", "MovingCommand",
            (object[] args) => new MovingCommand((IMovableObject)args[0])).Execute();


        // Act
        var command = Ioc.Resolve<ICommand>("MovingCommand", movableAdapter);

        // Assert
        Assert.IsType<MovingCommand>(command);
    }

    [Fact]
    public void InitAndClear()
    {
        //Arrange
        var universalObject = StarshipBuilder
            .CreateObject()
            .SetLocation(0, 0)
            .SetVelocity(1, 0);
        var movableAdapter = new MovableAdapter(universalObject);
        Ioc.Resolve<ICommand>("IoC.NewScope").Execute();
        Ioc.Resolve<ICommand>("IoC.Register", "MovingCommand",
            (object[] args) => new MovingCommand((IMovableObject)args[0])).Execute();

        // Act
        Ioc.Resolve<ICommand>("IoC.Clear").Execute();
        Assert.Throws<DependencyNotFoundException>(() => Ioc.Resolve<ICommand>("MovingCommand", movableAdapter));
    }

    [Fact]
    public void ResolveSearchInParentDependency()
    {
        //Arrange
        var universalObject = StarshipBuilder
            .CreateObject()
            .SetLocation(0, 0)
            .SetVelocity(1, 0)
            .SetFuelAmount(100)
            .SetFuelConsumption(1);
        var movableAdapter = new MovableAdapter(universalObject);
        var fuelConsumptionAdapter = new FuelConsumptionAdapter(universalObject);
        Ioc.Resolve<ICommand>("IoC.NewScope").Execute();
        Ioc.Resolve<ICommand>("IoC.Register", "CheckFuelCommand",
            (object[] args) => new CheckFuelCommand((IFuelConsumption)args[0], (IMovableObject)args[1])).Execute();
        Ioc.Resolve<ICommand>("IoC.NewScope").Execute();
        Ioc.Resolve<ICommand>("IoC.NewScope").Execute();

        // Act
        var command = Ioc.Resolve<ICommand>("CheckFuelCommand", fuelConsumptionAdapter, movableAdapter);

        // Assert
        Assert.IsType<CheckFuelCommand>(command);
    }

    [Fact]
    public Task ResolveDependencyMultiThreads()
    {
        var task1 = Task.Run(() =>
        {
            //Arrange
            var universalObject = StarshipBuilder
                .CreateObject()
                .SetLocation(0, 0)
                .SetVelocity(1, 0);
            var movableAdapter = new MovableAdapter(universalObject);
            Ioc.Resolve<ICommand>("IoC.NewScope").Execute();
            Ioc.Resolve<ICommand>("IoC.Register", "SameName",
                (object[] args) => new MovingCommand((IMovableObject)args[0])).Execute();

            // Act
            var command = Ioc.Resolve<ICommand>("SameName", movableAdapter);
            // Assert
            Assert.IsType<MovingCommand>(command);
        });

        var task2 = Task.Run(() =>
        {
            //Arrange
            var universalObject = StarshipBuilder
                .CreateObject()
                .SetLocation(0, 0)
                .SetVelocity(1, 0)
                .SetFuelAmount(100)
                .SetFuelConsumption(1);
            var movableAdapter = new MovableAdapter(universalObject);
            var fuelConsumptionAdapter = new FuelConsumptionAdapter(universalObject);
            Ioc.Resolve<ICommand>("IoC.NewScope").Execute();
            Ioc.Resolve<ICommand>("IoC.Register", "SameName",
                (object[] args) => new CheckFuelCommand((IFuelConsumption)args[0], (IMovableObject)args[1])).Execute();

            // Act
            var command = Ioc.Resolve<ICommand>("SameName", fuelConsumptionAdapter, movableAdapter);

            // Assert
            Assert.IsType<CheckFuelCommand>(command);
        });

        return Task.WhenAll([task1, task2]);
    }

    public void Dispose()
    {
    }
}