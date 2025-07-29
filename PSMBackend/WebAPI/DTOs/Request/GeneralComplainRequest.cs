using System;
using PSMModel.Enums;

namespace PSMWebAPI.DTOs.Request;

public class GeneralComplainAddRequest
{
    public string Name { get; set; }
    public string Detail { get; set; }
    public WorkpackageStatus? Status { get; set; }
    public bool IsPrivate { get; set; }
    public int ClientId { get; set; }
}
