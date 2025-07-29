using System;

namespace PSMModel.Models;

public class Company : BaseEntity
{
    public int CompanyId { get; set; }
    public string Name { get; set; }
    public string AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string City { get; set; }
    public string Mobile { get; set; }
}
