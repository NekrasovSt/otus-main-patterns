namespace Lessons.Dto;

public class ObjectAddedEvent
{
    public Guid GameId { get; set; }
    public Guid ObjectId { get; set; }
}