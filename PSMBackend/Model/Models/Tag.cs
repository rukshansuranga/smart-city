using System.ComponentModel.DataAnnotations;

namespace PSMModel.Models;

public class Tag
{
    [Key]
    public int TagId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string? Description { get; set; }
    
    [MaxLength(20)]
    public string? Color { get; set; } // For UI display (e.g., "#FF5733")
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual ICollection<TicketTag> TicketTags { get; set; } = new List<TicketTag>();
    public virtual ICollection<ComplainTag> ComplainTags { get; set; } = new List<ComplainTag>();
    public virtual ICollection<ProjectTag> ProjectTags { get; set; } = new List<ProjectTag>();
}
