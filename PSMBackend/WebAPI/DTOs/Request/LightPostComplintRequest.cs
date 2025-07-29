using System;
using PSMModel.Enums;

namespace PSMWebAPI.DTOs.Request;

public class LightPostComplintRequest
{
    public string Name { get; set; }
    public string Detail { get; set; }
    public WorkpackageStatus? Status { get; set; }
    public string LightPostNumber { get; set; }
    public int ClientId { get; set; }
}
