using System.Collections.Concurrent;
using Autofac;
using Lessons.Infrastructure;

namespace Lessons.States;

public class MoveToState : IServerState
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
    }

    public void Normal()
    {
        var state = _server.DI.ResolveNamed<IServerState>(nameof(NormalState));
        state.Handle(_server);
    }

    public void Handle(StateServer server)
    {
        _server = server;
        server.SetState(this);
    }
    
    public string Name => nameof(MoveToState);
}