using System;
using NodaTime;

namespace PSMWebAPI.DTOs.tender;

public class TenderPostRequest
{
    public required string Subject { get; set; }
    public string? Note { get; set; }
    public decimal BidAmount { get; set; }
    public IFormFile? TenderDocument { get; set; }
    public LocalDateTime SubmittedDate { get; set; }
    public int ProjectId { get; set; }
    public required string ContractorId { get; set; }

}
