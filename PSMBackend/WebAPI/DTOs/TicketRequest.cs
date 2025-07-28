using System;

namespace PSMWebAPI.DTOs;

public class TicketRequest
{
    public int? TicketId { get; set; }
    public string Title { get; set; }
    public string? Detail { get; set; }
    public string? Note { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public List<int>? WorkpackageIdList { get; set; }
    public int UserId { get; set; }

}
