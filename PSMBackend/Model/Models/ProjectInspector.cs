using System;
using System.ComponentModel.DataAnnotations.Schema;
using NodaTime;

namespace PSMModel.Models;

public class ProjectInspector: BaseEntity
{
    public int ProjectInspectorId { get; set; }
    [ForeignKey("Project")]
    public int ProjectId { get; set; }
    [ForeignKey("Inspector")]
    public string InspectorId { get; set; }
    public Project Project { get; set; }
    public User Inspector { get; set; }
    public LocalDate AssignDate { get; set; }
    public string? Responsibility { get; set; }
    public string? Note { get; set; }
}
