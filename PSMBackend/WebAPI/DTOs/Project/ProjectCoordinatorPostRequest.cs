using System;
using NodaTime;

namespace PSMWebAPI.DTOs.Project;

public class ProjectCoordinatorPostRequest
{
    public int ProjectId { get; set; }
    public required string CoordinatorId { get; set; }
    public LocalDate AssignDate { get; set; }
    public string? Responsibility { get; set; }
    public string? Note { get; set; }
}
