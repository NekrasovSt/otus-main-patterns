using System.Collections.Concurrent;
using Autofac;
using Lessons.Infrastructure;

namespace Lessons.States;

public class NormalState: IServerState
{
    private StateServer _server;
    public void Execute()
    {
        var item = _server.MainCollection.Take();
        try
        {
            item.Execute();
        }
        catch (Exception e)
        {
            _server.DI.Resolve<IExceptionHandler>().Execute(item, e, _server.MainCollection);
        }
    }

    public void Stop()
    {
        _server.SetState(null);
    }

    public void MoveTo()
    {
        var state = _server.DI.ResolveNamed<IServerState>(nameof(MoveToState));
        state.Handle(_server);
    }

    public void Normal()
    {
    }

    public void Handle(StateServer server)
    {
        _server = server;
        server.SetState(this);
    }

    public string Name => nameof(NormalState);
}