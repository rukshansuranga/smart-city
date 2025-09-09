using System.ComponentModel.DataAnnotations.Schema;

namespace PSMModel.Models;

public class ProjectTag
{
    [ForeignKey("Project")]
    public int ProjectId { get; set; }
    public virtual Project Project { get; set; } = null!;
    
    [ForeignKey("Tag")]
    public int TagId { get; set; }
    public virtual Tag Tag { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }
}
