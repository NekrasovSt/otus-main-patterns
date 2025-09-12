using System.Collections.Concurrent;

namespace Lessons.Infrastructure;

public interface IExceptionHandler
{
    public void Execute(ICommand cmd, Exception e, BlockingCollection<ICommand> collection)
    {
        
    }
}