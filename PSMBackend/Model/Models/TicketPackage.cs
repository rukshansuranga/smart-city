using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSMModel.Models;

public class TicketPackage
{
    public int Id { get; set; }
    [ForeignKey("Ticket")]
    public int TicketId { get; set; }
    public Ticket? Ticket { get; set; }
    [ForeignKey("Workpackage")]
    public int WorkpackageId { get; set; }
    public Workpackage? Workpackage { get; set; }
}
