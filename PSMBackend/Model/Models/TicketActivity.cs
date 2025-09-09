using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSMModel.Models;

public class TicketActivity : BaseEntity
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public TicketStatus? Transition { get; set; }
    public string? Note { get; set; }
    [ForeignKey("User")]
    public string UserId { get; set; }
    public User User { get; set; }
}
