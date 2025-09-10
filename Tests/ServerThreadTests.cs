using System.Collections.Concurrent;
using Lessons;
using Lessons.Adapters;
using Lessons.Commands;
using Lessons.Infrastructure;
using Moq;
using Xunit;

namespace Tests;

public class ServerThreadTests
{
    [Fact]
    public void StartCommand()
    {
        // Arrange
        var collection = new BlockingCollection<ICommand>();
        var serverThread = new ServerThread(collection);
        var emptyCommand = new Mock<ICommand>();
        emptyCommand.Setup(i => i.Execute()).Callback(serverThread.Stop);
        collection.Add(emptyCommand.Object);
        var command = new StartServerThreadCommand(serverThread);
        
        // Act
        command.Execute();
        
        // Assert
        serverThread.Wait();
        emptyCommand.Verify(i => i.Execute(), Times.Once);
    }

    [Fact]
    public void HandleExceptions()
    {
        var collection = new BlockingCollection<ICommand>();
        var serverThread = new ServerThread(collection);
        var emptyCommand1 = new Mock<ICommand>();
        collection.Add(emptyCommand1.Object);
        var errorCommand = new MovingCommand(new MovableAdapter(new UObject()));
        collection.Add(errorCommand);
        var emptyCommand2 = new Mock<ICommand>();
        emptyCommand2.Setup(i => i.Execute()).Callback(serverThread.Stop);
        collection.Add(emptyCommand2.Object);
        var handlerCommand = new Mock<ICommand>();
        Ioc.Resolve<ICommand>("IoC.Register", "ExceptionHandler", (object[] args) => handlerCommand.Object).Execute();
        
        // Act
        serverThread.Start();
        
        // Assert
        serverThread.Wait();
        emptyCommand1.Verify(i => i.Execute(), Times.Once);
        emptyCommand2.Verify(i => i.Execute(), Times.Once);
        handlerCommand.Verify(i => i.Execute(), Times.Once);
    }

    [Fact]
    public void HardStopCommand()
    {
        // Arrange
        var collection = new BlockingCollection<ICommand>();
        var serverThread = new ServerThread(collection);
        var emptyCommand1 = new Mock<ICommand>();
        var hardStopCommand = new HardStopCommand(serverThread);
        var emptyCommand2 = new Mock<ICommand>();
        collection.Add(emptyCommand1.Object);
        collection.Add(hardStopCommand);
        collection.Add(emptyCommand2.Object);
        
        // Act
        serverThread.Start();

        // Assert
        serverThread.Wait();
        emptyCommand1.Verify(i => i.Execute(), Times.Once);
        emptyCommand2.Verify(i => i.Execute(), Times.Never);
    }

    [Fact]
    public void SoftStopCommand()
    {
        // Arrange
        var collection = new BlockingCollection<ICommand>();
        var serverThread = new ServerThread(collection);
        var emptyCommand1 = new Mock<ICommand>();
        var hardStopCommand = new SoftStopCommand(serverThread);
        var emptyCommand2 = new Mock<ICommand>();
        emptyCommand2.Setup(i => i.Execute()).Callback(serverThread.Stop);
        collection.Add(emptyCommand1.Object);
        collection.Add(hardStopCommand);
        collection.Add(emptyCommand2.Object);
        
        // Act
        serverThread.Start();

        // Assert
        serverThread.Wait();
        emptyCommand1.Verify(i => i.Execute(), Times.Once);
        emptyCommand2.Verify(i => i.Execute(), Times.Once);
    }
}