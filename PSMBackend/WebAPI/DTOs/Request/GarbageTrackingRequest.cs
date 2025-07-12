using System;
using NodaTime;

namespace PSMWebAPI.DTOs.Request;

public class GarbageTrackingRequest
{
    public string RegionNo { get; set; }
    public LocalDate? Date { get; set; }
}
