using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PSMModel.Models;

public class Route
{
    [Key]
    public string RouteNo { get; set; }
    public string Name { get; set; }
    public float Distance { get; set; }


}
