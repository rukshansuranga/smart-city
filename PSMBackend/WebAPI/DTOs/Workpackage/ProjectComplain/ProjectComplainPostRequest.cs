using System;

namespace PSMWebAPI.DTOs.Workpackage.ProjectComplain;

public class ProjectComplainPostRequest
{
    
    public string Name { get; set; }
    public string Detail { get; set; }
    public int? ClientId { get; set; }
    public int? ProjectId { get; set; }
}
