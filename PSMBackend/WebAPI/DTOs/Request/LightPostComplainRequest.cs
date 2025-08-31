using System;
using PSMModel.Enums;

namespace PSMWebAPI.DTOs.Request;

public class LightPostComplainRequest
{
    public string Subject { get; set; }
    public string Detail { get; set; }
    public ComplainStatus? Status { get; set; }
    public string LightPostNumber { get; set; }
    public string ClientId { get; set; }
}
