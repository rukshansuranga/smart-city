using System;

namespace PSMModel.Models;

public class TicketHistory
{
        public int TicketHistoryId { get; set; }
        public int TicketId { get; set; }
        public string PropertyName { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public DateTime ChangedDate { get; set; }
        public int ChangedBy { get; set; } // UserId of who made the change

        public Ticket Ticket { get; set; }
}
