using System;

namespace PSMWebAPI.DTOs.Complain;

public class CircleRequest
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? Radius { get; set; }
    public int[]? Statuses { get; set; }
}
