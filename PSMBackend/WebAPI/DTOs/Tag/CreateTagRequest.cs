using System.ComponentModel.DataAnnotations;

namespace PSMWebAPI.DTOs.Tag;

public class CreateTagRequest
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string? Description { get; set; }
    
    [MaxLength(20)]
    public string? Color { get; set; }
    
    public bool IsActive { get; set; } = true;
}
