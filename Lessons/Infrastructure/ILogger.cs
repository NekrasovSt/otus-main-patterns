namespace Lessons.Infrastructure;

public interface ILogger
{
    void Log(Exception ex);
}