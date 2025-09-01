using System.Collections.Concurrent;

namespace Lessons.Infrastructure;

public class ServerThread
{
    private BlockingCollection<ICommand> _blockingCollection;
    private Action _behaviour;
    private Task _task;
    private bool _stop = false;
    private ManualResetEvent _manualReset = new ManualResetEvent(false);


    public int Length => _blockingCollection.Count;
    public Action Behaviour
    {
        get => _behaviour;
        set => _behaviour = value;
    }
    public ServerThread(BlockingCollection<ICommand> q)
    {
        _blockingCollection = q;

        _behaviour = MainLoop;

        _task = new Task(() =>
            {
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
                }
            }
        );
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
            Ioc.Resolve<ICommand>("ExceptionHandler", cmd, e, _blockingCollection).Execute();
        }
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

    public void UpdateBehaviour(Action newBehaviour)
    {
        _behaviour = newBehaviour;
    }
}