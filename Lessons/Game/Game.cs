using System.Collections.Concurrent;
using Autofac;
using Lessons.Commands;
using Lessons.Dto;
using Lessons.Infrastructure;
using MassTransit;

namespace Lessons.Game;

public class Game: IStoppable
{
    private readonly IBusControl _busControl;
    private IContainer _container;
    private readonly BlockingCollection<ICommand> _blockingCollection = new();
    private ManualResetEvent _manualReset = new ManualResetEvent(false);
    private Task _task;
    private bool _stop;
    private Action _behaviour;
    private Dictionary<Guid, UObject> _gameObjects = new();
    public Guid Id { get; init; }

    public Action Behaviour
    {
        get => _behaviour;
        set => _behaviour = value;
    }

    public Dictionary<Guid, UObject> GameObjects => _gameObjects;
    public Game(Action<ContainerBuilder, BlockingCollection<ICommand>, Game> registerCommand, IBusControl busControl)
    {
        _busControl = busControl;
        var builder = new ContainerBuilder();
        registerCommand.Invoke(builder, _blockingCollection, this);
        builder.RegisterInstance<IBusControl>(_busControl);
        builder.RegisterInstance(this);

        _container = builder.Build();
        
        _behaviour = MainLoop;

        _task = new Task(() =>
            {
                
                _busControl.Publish(new GameStartedEvent() { Id = Id });
                _manualReset.Reset();
                try
                {
                    while (!_stop)
                    {
                        _behaviour();
                    }
                }
                finally
                {
                    _manualReset.Set();
                    _busControl.Publish(new GameEndedEvent() { Id = Id });
                }
            }
        );
    }

    public void ExecuteCommand(GameCommand gameCommand)
    {
        _blockingCollection.Add(new InterpretCommand(gameCommand.CommandName, _blockingCollection, gameCommand.Object, _container)
        {
            Args = gameCommand.Args
        });
    }
    public void Start()
    {
        _task.Start();
    }

    public void Stop()
    {
        _stop = true;
    }

    public void Wait()
    {
        _manualReset.WaitOne();
    }
    
    private void MainLoop()
    {
        ICommand cmd = _blockingCollection.Take();
        try
        {
            cmd.Execute();
        }
        catch (Exception e)
        {
            _container.Resolve<IExceptionHandler>().Execute(cmd, e, _blockingCollection);
        }
    }
}