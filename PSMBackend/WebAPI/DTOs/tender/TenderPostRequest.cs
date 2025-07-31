using System;
using NodaTime;

namespace PSMWebAPI.DTOs.tender;

public class TenderPostRequest
{
    public string Subject { get; set; }
    public string? Note { get; set; }
    public decimal BidAmount { get; set; }
    public LocalDateTime SubmittedDate { get; set; }
    public int ProjectId { get; set; }
    public int CompanyId { get; set; }

}
