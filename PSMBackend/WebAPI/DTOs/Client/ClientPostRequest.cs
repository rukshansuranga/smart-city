using System;

namespace PSMWebAPI.DTOs.Client;

public class ClientPostRequest
{
    public string ClientId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Mobile { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
}
