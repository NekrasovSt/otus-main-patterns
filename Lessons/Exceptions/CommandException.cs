namespace Lesson4.Exceptions;

public class CommandException: Exception
{
    public CommandException()
    {
    }

    public CommandException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}