using System;
using System.ComponentModel.DataAnnotations;

namespace PSMModel.Models;

public class Client : BaseEntity
{
    [Key]
    public int ClientId { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string Mobile { get; set; }
    public string? Email { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string City { get; set; }
   
}
