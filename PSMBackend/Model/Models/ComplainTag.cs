using System.ComponentModel.DataAnnotations.Schema;

namespace PSMModel.Models;

public class ComplainTag
{
    [ForeignKey("Complain")]
    public int ComplainId { get; set; }
    public virtual Complain Complain { get; set; } = null!;
    
    [ForeignKey("Tag")]
    public int TagId { get; set; }
    public virtual Tag Tag { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }
}
