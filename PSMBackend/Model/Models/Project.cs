using System;
using System.ComponentModel.DataAnnotations.Schema;
using NodaTime;

namespace PSMModel.Models;

public class Project: BaseEntity
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
    public int? AwadedTenderId { get; set; }
    //public Tender? AwadedTener { get; set; }
    public LocalDate? TenderOpeningDate { get; set; }
    public LocalDate? TenderClosingDate { get; set; }
    public ICollection<ProjectComplain> ProjectComplains { get; set; }
}
