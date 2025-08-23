using System;

namespace PSMWebAPI.DTOs.Ticket;

public class UpdateTicketPayload
{
    public int TicketId { get; set; }
    public List<int> WorkpackageIds { get; set; }
}
