using System;
using System.ComponentModel.DataAnnotations.Schema;
using NodaTime;

namespace PSMModel.Models;

public class Tender : BaseEntity
{
    public int TenderId { get; set; }
    public string Subject { get; set; }
    public string? Note { get; set; }
    public decimal BidAmount { get; set; }
    public LocalDateTime SubmittedDate { get; set; }
    [ForeignKey("Project")]
    public int ProjectId { get; set; }
    public Project Project { get; set; }
    [ForeignKey("Company")]
    public int CompanyId { get; set; }
    public Company Company { get; set; }
}
