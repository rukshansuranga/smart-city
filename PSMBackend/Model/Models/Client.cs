using System;
using System.ComponentModel.DataAnnotations;

namespace PSMModel.Models;

public class Client
{
    [Key]
    public int ClientId { get; set; }
    public string Name { get; set; }
    public string Mobile { get; set; }
}
