using System;
using System.ComponentModel.DataAnnotations;

namespace PSMModel.Models;

public class LightPost
{
    [Key]
    public string LightPostNumber { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
