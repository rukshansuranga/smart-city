using System;
using System.ComponentModel.DataAnnotations.Schema;
using NodaTime;


namespace PSMModel.Models;

public class Ticket
{
    public int TicketId { get; set; }
    public string Title { get; set; }
    public string? Detail { get; set; }
    public string? Note { get; set; }
    public LocalDateTime CreatedDate { get; set; }
    [ForeignKey("User")]
    public int UserId { get; set; }
    public User User { get; set; }
    //public List<WorkPackage>? WorkPackages { get; set; }
    public List<TicketPackage>? TicketPackages { get; set; }

}
