using Lessons.Infrastructure;

namespace Lessons;

public interface IServerState
{
    void Execute();

    void Stop();
    void MoveTo();

    void Normal();

    void Handle(StateServer server);

    string Name { get; }
}