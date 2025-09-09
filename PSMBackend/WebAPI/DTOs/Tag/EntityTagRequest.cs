namespace PSMWebAPI.DTOs.Tag;

public class EntityTagRequest
{
    public string EntityType { get; set; } = string.Empty; // "Ticket", "Complain", "Project"
    public int EntityId { get; set; }
    public List<int> TagIds { get; set; } = new();
}
