using System;
using System.ComponentModel.DataAnnotations.Schema;
using NodaTime;
using PSMModel.Enums;

namespace PSMModel.Models;

public class ProjectProgress : BaseEntity
{
    public int ProjectProgressId { get; set; }
    [ForeignKey("Project")]
    public int ProjectId { get; set; }
    public string? Summary { get; set; }
    public string? Description { get; set; }
    public LocalDate ProgressDate { get; set; }
    public double ProgressPercentage { get; set; }
    [ForeignKey("ApprovedByUser")]
    public string? ApprovedBy { get; set; }
    public LocalDate? ApprovedAt { get; set; }
    public string? ApprovedNote { get; set; }
    public ProjectProgressApprovedStatus? ProjectProgressApprovedStatus { get; set; }
    public Project Project { get; set; }
    public User? ApprovedByUser { get; set; }
}
