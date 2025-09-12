namespace Lessons.Dto;

public class GameCommand
{
    public Guid GameId { get; init; }
    public Guid Object { get; init; }
    public string CommandName { get; init; }
    public Dictionary<string, object> Args { get; set; } = new Dictionary<string, object>();
}