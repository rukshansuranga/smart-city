using System.ComponentModel.DataAnnotations.Schema;

namespace PSMModel.Models;

public class TicketTag
{
    [ForeignKey("Ticket")]
    public int TicketId { get; set; }
    public virtual Ticket Ticket { get; set; } = null!;
    
    [ForeignKey("Tag")]
    public int TagId { get; set; }
    public virtual Tag Tag { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }
}
