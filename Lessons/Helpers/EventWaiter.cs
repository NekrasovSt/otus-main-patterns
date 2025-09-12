using MassTransit;

namespace Lessons.Helpers;

public class EventWaiter<T>: IConsumer<T> where T: class
{
    private readonly Predicate<T>? _predicate;

    public EventWaiter()
    {
    }

    public EventWaiter(Predicate<T> predicate)
    {
        _predicate = predicate;
    }

    private ManualResetEvent _manualResetEvent = new(false);
    public T Event { get; set; }
    public Task Consume(ConsumeContext<T> context)
    {
        if (_predicate != null && !_predicate.Invoke(context.Message))
        {
            return Task.CompletedTask;
        }
        Event = context.Message;
        _manualResetEvent.Set();
        return Task.CompletedTask;
    }

    public void Wait()
    {
        _manualResetEvent.WaitOne();
    }
}