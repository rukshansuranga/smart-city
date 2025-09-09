using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSMModel.Models;

public class TicketComplain
{
    public int Id { get; set; }
    [ForeignKey("Ticket")]
    public int TicketId { get; set; }
    public Ticket? Ticket { get; set; }
    [ForeignKey("Complain")]
    public int ComplainId { get; set; }
    public Complain? Complain { get; set; }
}
