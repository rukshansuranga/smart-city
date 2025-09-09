using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NodaTime;
using PSMModel.Enums;

namespace PSMModel.Models;

public class Complain : BaseEntity, IAttachable
{
    [Key]
    public int ComplainId { get; set; }
    public string Subject { get; set; }
    public string? Detail { get; set; }
    public double? Rating { get; set; }
    public string? RatedBy { get; set; }
    public LocalDateTime? RatedAt { get; set; }
    public string? RatingReview { get; set; }
    public string? Sentiment { get; set; }
    public string? Summary { get; set; }
    public int MyProperty { get; set; }
    public ComplainStatus? Status { get; set; }
    [ForeignKey("Client")]
    public string? ClientId { get; set; }
    public Client? Client { get; set; }
    public string ComplainType { get; set; }
    public List<TicketComplain>? TicketComplains { get; set; }
    
    // Tag relationships
    public virtual ICollection<ComplainTag> ComplainTags { get; set; } = new List<ComplainTag>();
    
    // Navigation property for attachments
    public virtual ICollection<Attachment>? Attachments { get; set; }
    
    // IAttachable implementation
    public int GetEntityId() => ComplainId;
    public string GetEntityType() => nameof(Complain);
}
