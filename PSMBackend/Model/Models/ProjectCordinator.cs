using System;
using System.ComponentModel.DataAnnotations.Schema;
using NodaTime;
using PSMModel.Enums;

namespace PSMModel.Models;

public class ProjectCoordinator: BaseEntity
{
    public int ProjectCoordinatorId { get; set; }
    [ForeignKey("Project")]
    public int ProjectId { get; set; }
    [ForeignKey("Coordinator")]
    public required string CoordinatorId { get; set; }
    public Project? Project { get; set; }
    public User? Coordinator { get; set; }
    public LocalDate AssignDate { get; set; }
    public ProjectCoordinatorType CoordinatorType { get; set; }
    public string? Note { get; set; }
}
