namespace Lessons.Dto;

public class CommandExecutedEvent
{
    public required Guid GameId { get; init; }
    public required string CommandName { get; init; }
}