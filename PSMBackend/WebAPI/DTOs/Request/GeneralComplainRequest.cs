using System;

namespace PSMWebAPI.DTOs.Request;

public class GeneralComplainAddRequest
{
    public string Name { get; set; }
    public string Detail { get; set; }
    public string? Status { get; set; }
    public bool IsPrivate { get; set; }
    public int ClientId { get; set; }
}
