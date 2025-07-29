using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class TicketPackageMappingConfiguration : IEntityTypeConfiguration<TicketPackage>
{
    public void Configure(EntityTypeBuilder<TicketPackage> builder)
    {
        builder.HasData(
                new TicketPackage
                {
                    Id = 1,
                    TicketId = 1,
                    WorkpackageId = 2
                },
                new TicketPackage
                {  
                    Id = 2,
                    TicketId = 1,
                    WorkpackageId = 3
                },
                new TicketPackage
                {
                    Id = 3,
                    TicketId = 3,
                    WorkpackageId = 4
                },
                new TicketPackage
                {
                    Id = 4,
                    TicketId = 4,
                    WorkpackageId = 5
                }
            );
    }
}
