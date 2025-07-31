using System;
using PSMModel.Enums;

namespace PSMWebAPI.DTOs.Workpackage.GeneralComplain;

public class GeneralComplainUpdateRequest
{
    public int WorkpackageId { get; set; }
    public string Subject { get; set; }
    public string? Detail { get; set; }
    public WorkpackageStatus? Status { get; set; }
    public bool IsPrivate { get; set; }
    public int ClientId { get; set; }
}
