using System.Collections.Concurrent;
using Autofac;
using Lessons;
using Lessons.Commands;
using Lessons.Infrastructure;
using Lessons.States;
using Xunit;

namespace Tests;

public class StateServerTests
{
    private IContainer GetContainer()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<NormalState>().Named<IServerState>(nameof(NormalState));
        builder.RegisterType<MoveToState>().Named<IServerState>(nameof(MoveToState));
        return builder.Build();
    }
    
    [Fact]
    public void HardStop()
    {
        // Arrange
        var mainCollection = new BlockingCollection<ICommand>();
        var backupCollection = new BlockingCollection<ICommand>();
        var container = GetContainer();
        var server = new StateServer(mainCollection, backupCollection, container);
        mainCollection.Add(new HardStopCommand(server));

        // Act
        server.Start();
        server.Wait();
     
        // Assert
        Assert.Empty(mainCollection);
        Assert.Equal("Unknown", server.State);
    }
    
    [Fact]
    public void MoveTo()
    {
        // Arrange
        var mainCollection = new BlockingCollection<ICommand>();
        var backupCollection = new BlockingCollection<ICommand>();
        var container = GetContainer();
        var server = new StateServer(mainCollection, backupCollection, container);
        var history = new List<string>();
        mainCollection.Add(new MoveToCommand(server));
        mainCollection.Add(new HardStopCommand(server));
        server.ChangeStare += history.Add;

        // Act
        server.Start();
        server.Wait();
     
        // Assert
        Assert.Empty(mainCollection);
        Assert.Equal("Unknown", server.State);
        Assert.Equal([nameof(NormalState), nameof(MoveToState), "Unknown"], history);
    }
    
    [Fact]
    public void FromMoveToNormal()
    {
        // Arrange
        var mainCollection = new BlockingCollection<ICommand>();
        var backupCollection = new BlockingCollection<ICommand>();
        var container = GetContainer();
        var server = new StateServer(mainCollection, backupCollection, container);
        var history = new List<string>();
        mainCollection.Add(new MoveToCommand(server));
        mainCollection.Add(new RunCommand(server));
        mainCollection.Add(new HardStopCommand(server));
        server.ChangeStare += history.Add;

        // Act
        server.Start();
        server.Wait();
     
        // Assert
        Assert.Empty(mainCollection);
        Assert.Equal("Unknown", server.State);
        Assert.Equal([nameof(NormalState), nameof(MoveToState), nameof(NormalState), "Unknown"], history);
    }
}