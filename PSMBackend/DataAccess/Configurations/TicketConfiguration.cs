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
                UserId = "0c895075-b8e6-48f9-bb9e-2c9db9d7207a",
                Subject = "Fix Light Post LP001",
                Status = TicketStatus.Open,
                CreatedAt = new LocalDateTime(2025,6,19,14,14,0),
                CreatedBy = "0c895075-b8e6-48f9-bb9e-2c9db9d7207a",
                IsActive = true
            },
            new Ticket
            {
                TicketId = 2,
                UserId = "0c895075-b8e6-48f9-bb9e-2c9db9d7207a",
                Subject = "Fix Light Post LP001 2",
                Status = TicketStatus.Open,
                CreatedAt = new LocalDateTime(2025,6,19,14,14,0),
                CreatedBy = "0c895075-b8e6-48f9-bb9e-2c9db9d7207a",
                IsActive = true
            },
            new Ticket
            {
                TicketId = 3,
                UserId = "0c895075-b8e6-48f9-bb9e-2c9db9d7207a",
                Subject = "Fix Light Post LP001 3",
                Status = TicketStatus.Open,
                CreatedAt = new LocalDateTime(2025,6,19,14,14,0),
                CreatedBy = "0c895075-b8e6-48f9-bb9e-2c9db9d7207a",
                IsActive = true
            },
            new Ticket
            {
                TicketId = 4,
                UserId = "43e63068-a5fd-4a45-acfb-0383ff4d45ea",
                Subject = "Fix Light Post LP001 4",
                Status = TicketStatus.Open,
                CreatedAt = new LocalDateTime(2025,6,19,14,14,0),
                CreatedBy = "43e63068-a5fd-4a45-acfb-0383ff4d45ea",
                IsActive = true
            }
        );
    }
}
