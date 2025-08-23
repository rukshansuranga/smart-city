using System;

namespace PSMWebAPI.DTOs.Workpackage;

public class RatingRequest
{
    public int NotificationId { get; set; }
    public int WorkpackageId { get; set; }
    public int Rating { get; set; }
    public string Note { get; set; } = string.Empty;
    public string ClientId { get; set; }
}
