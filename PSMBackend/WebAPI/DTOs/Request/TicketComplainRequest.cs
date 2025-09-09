using System;

namespace PSMWebAPI.DTOs.Request;

public class TicketComplainRequest
{
    public int TicketId { get; set; }
    public int ComplainId { get; set; }
    public string Action { get; set; }
}
