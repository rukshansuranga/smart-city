using System;
using PSMModel.Enums;

namespace PSMWebAPI.DTOs;

public class WorkpackageRequest
{
    public string Subject { get; set; }
    public string Detail { get; set; }
    public WorkpackageStatus? Status { get; set; }
}
