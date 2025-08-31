using System;
using NodaTime;

namespace PSMModel.Models;

public class ProjectInspection : BaseEntity
{
    public int ProjectInspectionId { get; set; }
    public int ProjectInspectorId { get; set; }
    public ProjectInspector ProjectInspector { get; set; }
    public LocalDateTime InspectionDate { get; set; }
    public string? Findings { get; set; }
    public string? Recommendations { get; set; }
    public int? TicketId { get; set; }
}
