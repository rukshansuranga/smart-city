namespace PSMWebAPI.DTOs.Tag;

public class TagDto
{
    public int TagId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Color { get; set; }
    public bool IsActive { get; set; }
}
