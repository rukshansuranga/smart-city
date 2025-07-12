using System;

namespace PSMWebAPI.DTOs;

public class WorkpackageRequest
{
    public string Name { get; set; }
    public string Detail { get; set; }
    public string? Status { get; set; }
}
