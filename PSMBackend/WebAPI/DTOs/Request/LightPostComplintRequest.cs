using System;

namespace PSMWebAPI.DTOs.Request;

public class LightPostComplintRequest
{
    public string Name { get; set; }
    public string Detail { get; set; }
    public string? Status { get; set; }
    public string LightPostNumber { get; set; }
    public int ClientId { get; set; }
}
