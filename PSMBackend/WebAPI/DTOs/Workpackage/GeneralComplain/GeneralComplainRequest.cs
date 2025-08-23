using System;
using PSMModel.Enums;

namespace PSMWebAPI.DTOs.Workpackage.GeneralComplain;

public class GeneralComplainAddRequest
{
    public string Subject { get; set; }
    public string Detail { get; set; }
    public WorkpackageStatus? Status { get; set; }
    public bool IsPrivate { get; set; }
    public string ClientId { get; set; }
}
