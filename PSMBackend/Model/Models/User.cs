using System;
using System.ComponentModel.DataAnnotations;

namespace PSMModel.Models;

public class User
{
    [Key]
    public int UserId { get; set; }
    public string TelNumber { get; set; }
    public string Name { get; set; }
}
