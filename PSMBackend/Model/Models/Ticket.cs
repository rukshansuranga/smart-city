using System;
using System.ComponentModel.DataAnnotations.Schema;
using NodaTime;
using PSMModel.Enums;


namespace PSMModel.Models;

public class Ticket: BaseEntity
{
    public int TicketId { get; set; }
    public string Subject { get; set; }
    public string? Detail { get; set; }
    public string? Note { get; set; }
    public TicketStatus? Status { get; set; }
    public TicketType? Type { get; set; }
    public TicketWorkpackageType? TicketWorkpackageType { get; set; }
    public int Estimation { get; set; }
    public TicketPriority? Priority { get; set; }
    public LocalDate? DueDate { get; set; }
    public List<string>? Attachments { get; set; }

    [ForeignKey("User")]
    public string? UserId { get; set; }
    public User? User { get; set; }
    //public List<WorkPackage>? WorkPackages { get; set; }
    public List<TicketPackage>? TicketPackages { get; set; }
    public List<TicketActivity>? Activities { get; set; }

}
