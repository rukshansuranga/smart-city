using System;
using PSMModel.Enums;

namespace PSMModel.Models;

public class ComplainTicket : Ticket
{
    public ComplainType? ComplainType { get; set; }
    public List<TicketPackage>? TicketPackages { get; set; }
}
