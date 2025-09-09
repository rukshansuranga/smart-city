using System;
using PSMModel.Models;

namespace PSMWebAPI.DTOs.Ticket;

public class BoardTicket
{
    public List<ProjectTicket> ProjectTickets { get; set; }
    public List<ComplainTicket> ComplainTickets { get; set; }
    public List<InternalTicket> InternalTickets { get; set; }
}
