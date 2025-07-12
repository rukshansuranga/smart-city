using System;

namespace PSMWebAPI.DTOs.Response;

public class LightPostComplainDetail
{
    public int WorkPackageId { get; set; }
    public string Name { get; set; }
    public string ClientName { get; set; }
    public string Status { get; set; }
    public string ComplainDate { get; set; }
    public List<TicketResponse> TicketList { get; set; }
}

public class TicketResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
}
