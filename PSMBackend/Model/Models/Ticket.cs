using System;
using System.ComponentModel.DataAnnotations.Schema;
using NodaTime;
using PSMModel.Enums;


namespace PSMModel.Models;

public class Ticket: BaseEntity, IAttachable
{
    public int TicketId { get; set; }
    public string Subject { get; set; }
    public string? Detail { get; set; }
    public string? Note { get; set; }
    public TicketStatus? Status { get; set; }
    public int Estimation { get; set; }
    public TicketPriority? Priority { get; set; }
    public LocalDate? DueDate { get; set; }
    [ForeignKey("User")]
    public string? UserId { get; set; }
    public User? User { get; set; }
    public List<TicketActivity>? Activities { get; set; }
    
    // Tag relationships
    public virtual ICollection<TicketTag> TicketTags { get; set; } = new List<TicketTag>();
    
    // Navigation property for attachments
    public virtual ICollection<Attachment>? Attachments { get; set; }
    
    // IAttachable implementation
    public int GetEntityId() => TicketId;
    public string GetEntityType() => nameof(Ticket);
}
