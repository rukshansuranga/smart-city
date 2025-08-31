using System;
using NodaTime;
using PSMModel.Enums;

namespace PSMWebAPI.DTOs.Project;

public class ProjectProgressRequest
{
    public int ProjectId { get; set; }
    public string? Summary { get; set; }
    public string? Description { get; set; }
    public LocalDate ProgressDate { get; set; }
    public double ProgressPercentage { get; set; }
    public string? ApprovedBy { get; set; }
    public LocalDate? ApprovedAt { get; set; }
    public string? ApprovedNote { get; set; }
    public ProjectProgressApprovedStatus? ProjectProgressApprovedStatus { get; set; }
}
