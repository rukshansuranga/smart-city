using System;

namespace PSMWebAPI.DTOs.Complain.ProjectComplain;

public class ProjectComplainPostRequest
{
    
    public string Subject { get; set; }
    public string Detail { get; set; }
    public string? ClientId { get; set; }
    public int? ProjectId { get; set; }
}
