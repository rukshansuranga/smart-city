using System;
using NodaTime;

namespace PSMWebAPI.DTOs.Project;

public class ProjectPostRequest
{

    public string Subject { get; set; }
    public string Note { get; set; }
    public decimal BidAmount { get; set; }
    public int? ProjectId { get; set; }
    public int? CompanyId { get; set; }
    public LocalDate? SubmittedDate { get; set; }
    
}
 