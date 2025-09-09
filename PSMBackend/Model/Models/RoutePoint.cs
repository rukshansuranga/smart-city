using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSMModel.Models;

public class RoutePoint : BaseEntity
{
    [Key]
    public int RoutePointId { get; set; }
    public string PointName { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    [ForeignKey("Route")]
    public string RouteNo { get; set; }
    public Route Route { get; set; }
}
