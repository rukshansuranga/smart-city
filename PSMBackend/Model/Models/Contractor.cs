using System;

namespace PSMModel.Models;

public class Contractor : BaseEntity
{
    public required string ContractorId { get; set; }
    public required string Name { get; set; }
    public required string AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public required string City { get; set; }
    public required string Mobile { get; set; }
}
