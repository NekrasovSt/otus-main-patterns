using System.Collections.Concurrent;
using Lessons;
using Lessons.Adapters;
using Lessons.Commands;
using Lessons.Exceptions;
using Lessons.Infrastructure;
using Moq;
using Xunit;

namespace Tests;

public class CommandExecutorTests
{
    [Fact]
    public void ExecutionWithoutErrors()
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
        var logger = new Mock<ILogger>();
        var blockingCollection = new BlockingCollection<ICommand>();
        var commandExecutor =
            new CommandExecutor(blockingCollection, new ExecutionHandler(blockingCollection, logger.Object));
        blockingCollection.Add(new MovingCommand(movableAdapter));
        //Act
        commandExecutor.ExecuteSingleCommand();

        // Assert
        Assert.Equal(movableAdapter.Location.X, 5);
        Assert.Equal(movableAdapter.Location.Y, 8);
    }

    [Fact]
    public void WriteLogCommand()
    {
        // Arrange
        var logger = new Mock<ILogger>();
        var customException = new Exception();
        var command = new WriteLogCommand(logger.Object, customException);

        //Act
        command.Execute();

        //Assert
        logger.Verify(i => i.Log(customException), Times.Once());
    }

    [Fact]
    public void EnqueueCommand()
    {
        // Arrange
        var blockingCollection = new BlockingCollection<ICommand>();
        var targetCommand = new Mock<ICommand>();
        var enqueue = new EnqueueCommand(blockingCollection, targetCommand.Object);

        //Act
        enqueue.Execute();

        // Assert
        Assert.Single(blockingCollection);
    }

    [Fact]
    public void RetryCommand()
    {
        // Arrange
        var targetCommand = new Mock<ICommand>();
        var retry = new RetryCommand(targetCommand.Object, 0);

        // Act
        retry.Execute();

        // Assert
        targetCommand.Verify(i => i.Execute(), Times.Once());
    }

    [Fact]
    public void ErrorChain1()
    {
        // при первом выбросе исключения повторить команду, при повторном выбросе исключения записать информацию в лог.
        // Arrange
        var universalObject = new UObject();
        var rotatable = new RotatableAdapter(universalObject);

        var logger = new Mock<ILogger>();
        var blockingCollection = new BlockingCollection<ICommand>();
        var commandExecutor =
            new CommandExecutor(blockingCollection, new ExecutionHandler(blockingCollection, logger.Object));
        var rotateCommand = new RotateCommand(rotatable);
        blockingCollection.Add(rotateCommand);

        // Act
        commandExecutor.ExecuteSingleCommand();

        Assert.Single(blockingCollection); // В очереди RetryCommand

        commandExecutor.ExecuteSingleCommand();

        // Assert
        Assert.Empty(blockingCollection);
        logger.Verify(i => i.Log(It.IsAny<NotRotatableObjectException>()));
    }

    [Fact]
    public void ErrorChain2()
    {
        // Реализовать стратегию обработки исключения - повторить два раза, потом записать в лог.
        // Arrange
        var universalObject = new UObject();
        var movable = new MovableAdapter(universalObject);
        var logger = new Mock<ILogger>();
        var blockingCollection = new BlockingCollection<ICommand>();
        var movingCommand = new MovingCommand(movable);
        var commandExecutor =
            new CommandExecutor(blockingCollection, new ExecutionHandler(blockingCollection, logger.Object));
        blockingCollection.Add(movingCommand);
        
        // Act
        commandExecutor.ExecuteSingleCommand();

        Assert.Single(blockingCollection); // В очереди RetryCommand.NumberOfRetries = 0

        commandExecutor.ExecuteSingleCommand();
        
        Assert.Single(blockingCollection); // В очереди RetryCommand.NumberOfRetries = 1

        commandExecutor.ExecuteSingleCommand();

        // Assert
        Assert.Empty(blockingCollection);
        logger.Verify(i => i.Log(It.IsAny<NotMovableObjectException>()));
    }
}