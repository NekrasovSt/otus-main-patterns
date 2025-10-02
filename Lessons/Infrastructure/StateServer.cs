using System.Collections.Concurrent;
using Autofac;
using Lessons.States;

namespace Lessons.Infrastructure;

public class StateServer : IStoppable
{
    private BlockingCollection<ICommand> _blockingCollection;
    private BlockingCollection<ICommand> _backUpCollection;
    private IServerState? _serverState;
    private readonly Task _task;
    private IContainer _container;
    private ManualResetEvent _manualReset = new ManualResetEvent(false);

    public StateServer(BlockingCollection<ICommand> blockingCollection, BlockingCollection<ICommand> backUpCollection, IContainer container)
    {
        _blockingCollection = blockingCollection ?? throw new ArgumentNullException(nameof(blockingCollection));
        _backUpCollection = backUpCollection ?? throw new ArgumentNullException(nameof(backUpCollection));
        _container = container ?? throw new ArgumentNullException(nameof(container));

        _task = new Task(MainLoop);
    }
    
    public event Action<string> ChangeStare;
    public void Start()
    {
        _serverState = _container.ResolveNamed<IServerState>(nameof(NormalState));
        _serverState.Handle(this);
        _task.Start();
    }

    internal BlockingCollection<ICommand> MainCollection => _blockingCollection;
    internal BlockingCollection<ICommand> BackupCollection => _backUpCollection;
    internal IContainer DI => _container;
    internal void SetState(IServerState? serverState)
    {
        _serverState = serverState;
        ChangeStare?.Invoke(serverState?.Name ?? "Unknown");
    }
    private void MainLoop()
    {
        _manualReset.Reset();
        try
        {
            while (_serverState != null)
            {
                _serverState.Execute();
            }
        }
        finally
        {
            _manualReset.Set();
        }
    }

    public void Normal()
    {
        _serverState?.Normal();
    }
    
    public void MoveTo()
    {
        _serverState?.MoveTo();
    }

    public void Stop()
    {
        _serverState?.Stop();
    }

    public string State => _serverState?.Name ?? "Unknown";

    public void Wait()
    {
        _manualReset.WaitOne();
    }
}