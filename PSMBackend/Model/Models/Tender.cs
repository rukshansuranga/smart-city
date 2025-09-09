using System;
using System.ComponentModel.DataAnnotations.Schema;
using NodaTime;

namespace PSMModel.Models;

public class Tender : BaseEntity
{
    public int TenderId { get; set; }
    public required string Subject { get; set; }
    public string? Note { get; set; }
    public decimal BidAmount { get; set; }
    public string? TenderDocument { get; set; }
    public LocalDateTime? SubmittedDate { get; set; }
    [ForeignKey("Project")]
    public int ProjectId { get; set; }
    public Project? Project { get; set; }
    [ForeignKey("Contractor")]
    public required string ContractorId { get; set; }
    public Contractor? Contractor { get; set; }
}
