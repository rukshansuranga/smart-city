using System;
using Microsoft.AspNetCore.Http;
using NodaTime;

namespace PSMWebAPI.DTOs.Project;

public class ProjectPostRequest
{
    public required string Subject { get; set; }
    public string? Description { get; set; }
    public decimal EstimatedCost { get; set; }
    public int? ProjectId { get; set; }
    public string? ContractorId { get; set; }
    public LocalDate? StartDate { get; set; }
    public LocalDate? EndDate { get; set; }
    public LocalDate? TenderOpeningDate { get; set; }
    public LocalDate? TenderClosingDate { get; set; }
    public string? City { get; set; }
    public ProjectType? Type { get; set; }
    public ProjectStatus? Status { get; set; }
    public IFormFile? SpecificationDocument { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? Location { get; set; }
    public string? LocationNote { get; set; }
    public int? AwardedTenderId { get; set; }

}
 