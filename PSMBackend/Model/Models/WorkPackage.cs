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
    public double? Rating { get; set; }
    public string? RatedBy { get; set; }
    public LocalDateTime? RatedAt { get; set; }
    public string? RatingReview { get; set; }
    public string? Sentiment { get; set; }
    public string? Summary { get; set; }
    public int MyProperty { get; set; }
    public WorkpackageStatus? Status { get; set; }
    [ForeignKey("Client")]
    public string? ClientId { get; set; }
    public Client? Client { get; set; }
    public string WorkpackageType { get; set; }
    public List<TicketPackage>? TicketPackages { get; set; }
    public List<Comment>? Comments { get; set; }
}
