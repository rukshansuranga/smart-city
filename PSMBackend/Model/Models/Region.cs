using System;
using System.ComponentModel.DataAnnotations;

namespace PSMModel.Models;

public class Region: BaseEntity
{
    [Key]
    public string RegionNo { get; set; }
    public string Name { get; set; }
    public string? Note { get; set; }
}
