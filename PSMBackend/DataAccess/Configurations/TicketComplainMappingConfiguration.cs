using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class TicketComplainMappingConfiguration : IEntityTypeConfiguration<TicketComplain>
{
    public void Configure(EntityTypeBuilder<TicketComplain> builder)
    {
        builder.HasData(
                new TicketComplain
                {
                    Id = 1,
                    TicketId = 1,
                    ComplainId = 2
                },
                new TicketComplain
                {  
                    Id = 2,
                    TicketId = 1,
                    ComplainId = 3
                },
                new TicketComplain
                {
                    Id = 3,
                    TicketId = 3,
                    ComplainId = 4
                },
                new TicketComplain
                {
                    Id = 4,
                    TicketId = 4,
                    ComplainId = 5
                }
            );
    }
}
