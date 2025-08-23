using System;
using NodaTime;

namespace PSMWebAPI.DTOs.Response;

public class LightPostComplainDetail
{
    public int WorkpackageId { get; set; }
    public string Subject { get; set; }
    public string ClientId { get; set; }
    public string ClientName { get; set; }
    public string Status { get; set; }
    public LocalDateTime ComplainDate { get; set; }
    public List<TicketResponse> TicketList { get; set; }
}

public class TicketResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
}
