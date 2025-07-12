using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NodaTime;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        //feed ticket
        builder.HasData(
            new Ticket
            {
                TicketId = 1,
                UserId = 1,
                Title = "Fix Light Post LP001",
                CreatedDate = new LocalDateTime(2025,6,19,14,14)
            },
            new Ticket
            {
                TicketId = 2,
                UserId = 2,
                Title = "Fix Light Post LP001 2",
                CreatedDate = new LocalDateTime(2025,6,19,14,14)
            },
            new Ticket
            {
                TicketId = 3,
                UserId = 2,
                Title = "Fix Light Post LP001 3",
                CreatedDate = new LocalDateTime(2025,6,19,14,14)
            },
            new Ticket
            {
                TicketId = 4,
                UserId = 2,
                Title = "Fix Light Post LP001 4",
                CreatedDate = new LocalDateTime(2025,6,19,14,14)
            }
        );
    }
}
