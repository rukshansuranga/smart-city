using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NodaTime;
using PSMModel.Enums;

namespace PSMModel.Models;

public class Workpackage :BaseEntity
{
    [Key]
    public int WorkpackageId { get; set; }
    public string Subject { get; set; }
    public string? Detail { get; set; }
    public WorkpackageStatus? Status { get; set; }
    [ForeignKey("Client")]
    public int? ClientId { get; set; }
    public Client? Client { get; set; }
    public string WorkpackageType { get; set; }
    public List<TicketPackage>? TicketPackages { get; set; }
    public List<Comment>? Comments { get; set; }
}
