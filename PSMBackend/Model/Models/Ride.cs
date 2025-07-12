using System;
using System.ComponentModel.DataAnnotations.Schema;
using NodaTime;

namespace PSMModel.Models;

public class Ride
{
    public int Id { get; set; }
    public LocalDateTime StartTime { get; set; }
    public LocalDateTime? EndTime { get; set; }
    public string Type { get; set; }
    public string? Notes { get; set; }

    [ForeignKey("Driver")]
    public int DriverNo { get; set; }
    public int Driver { get; set; }
    [ForeignKey("Vehical")]
    public string VehicalNo { get; set; }
    public Vehical Vehical { get; set; }
    [ForeignKey("Region")]
    public string RegionNo { get; set; }
    public Region Region { get; set; }
    public ICollection<RidePoint> RidePoints { get; set; }
}
