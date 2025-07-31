using System;
using NodaTime;

namespace PSMWebAPI.DTOs.Ticket;

public class TicketPostRequest
{
    public string Subject { get; set; }
    public string? Detail { get; set; }
    public string? Note { get; set; }
    public TicketType? Type { get; set; }
    public TicketStatus? Status { get; set; }
    public List<int>? WorkpackageIdList { get; set; }
    public int UserId { get; set; }
    public int Estimation { get; set; }
    public TicketPriority? Priority { get; set; }
    public LocalDate? DueDate { get; set; }
    public List<string>? Attachments { get; set; }

}
