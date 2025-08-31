using System;
using PSMModel.Enums;

namespace PSMWebAPI.DTOs.Complain.ProjectComplain;

public class ProjectComplainUpdateRequest
{
public int ComplainId { get; set; }
    public int? ProjectId { get; set; }
    public string Subject { get; set; }
    public string? Detail { get; set; }
    public ComplainStatus? Status { get; set; }
    public int ClientId { get; set; }
}
