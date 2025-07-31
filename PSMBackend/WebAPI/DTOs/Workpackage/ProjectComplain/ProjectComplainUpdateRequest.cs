using System;
using PSMModel.Enums;

namespace PSMWebAPI.DTOs.Workpackage.ProjectComplain;

public class ProjectComplainUpdateRequest
{
public int WorkpackageId { get; set; }
    public int? ProjectId { get; set; }
    public string Subject { get; set; }
    public string? Detail { get; set; }
    public WorkpackageStatus? Status { get; set; }
    public int ClientId { get; set; }
}
