using System;
using System.ComponentModel.DataAnnotations.Schema;
using PSMModel.Enums;

namespace PSMModel.Models;

public class Notification : BaseEntity
{
    public int Id { get; set; }
    public string Subject { get; set; }
    public string? Message { get; set; }
    public string? ClientId { get; set; }
    public string? UserId { get; set; }
    [ForeignKey("Ticket")]
    public int? TicketId { get; set; }
    public Ticket? Ticket { get; set; }
    [ForeignKey("Complain")]
    public int? ComplainId { get; set; }
    public Complain? Complain { get; set; }
    public NotificationStatus Status { get; set; }
    public NotificationType Type { get; set; }
    public bool IsRead { get; set; }
    public string? Error { get; set; }
    public int? RetryCount { get; set; }

} 
