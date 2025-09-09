using System;
using System.ComponentModel.DataAnnotations.Schema;
using NodaTime;
using PSMModel.Enums;

namespace PSMModel.Models;

public class Project: BaseEntity, IAttachable
{
    public int Id { get; set; }
    public string Subject { get; set; }
    public string? Description { get; set; }
    public ProjectType? Type { get; set; }
    public string? SpecificationDocument { get; set; }
    public LocalDate? StartDate { get; set; }
    public LocalDate? EndDate { get; set; }
    public ProjectStatus? Status { get; set; }
    public string? Location { get; set; }
    public string? LocationNote { get; set; }
    public string City { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public decimal? EstimatedCost { get; set; }
    //[ForeignKey("Tender")]
    public int? AwardedTenderId { get; set; }
    public ProjectProgressFrequency? ProgressFrequency { get; set; }
    //public Tender? AwardedTender { get; set; }
    public LocalDate? TenderOpeningDate { get; set; }
    public LocalDate? TenderClosingDate { get; set; }
    public ICollection<ProjectComplain>? ProjectComplains { get; set; }
    
    // Tag relationships
    public virtual ICollection<ProjectTag> ProjectTags { get; set; } = new List<ProjectTag>();
    
    // Navigation property for attachments
    public virtual ICollection<Attachment>? Attachments { get; set; }
    
    // IAttachable implementation
    public int GetEntityId() => Id;
    public string GetEntityType() => nameof(Project);
}
