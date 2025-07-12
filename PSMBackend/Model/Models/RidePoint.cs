using System;
using System.ComponentModel.DataAnnotations.Schema;
using NodaTime;

namespace PSMModel.Models;

public class RidePoint
{
    public int Id { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public LocalDateTime PointTime { get; set; }
    [ForeignKey("Ride")]
    public int RideId { get; set; }
    public Ride Ride { get; set; }
}
