using System;
using System.ComponentModel.DataAnnotations;

namespace PSMModel.Models;

public class Vehical: BaseEntity
{
    [Key]
    public string VehicalNo { get; set; }
    public string RegistrationNo { get; set; }
    public string Model { get; set; }
    public string Brand { get; set; }
    public string Year { get; set; }
}
