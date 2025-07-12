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
                    WorkPackageId = 2
                },
                new TicketPackage
                {
                    Id = 2,
                    TicketId = 1,
                    WorkPackageId = 3
                },
                new TicketPackage
                {
                    Id = 3,
                    TicketId = 3,
                    WorkPackageId = 4
                },
                new TicketPackage
                {
                    Id = 4,
                    TicketId = 4,
                    WorkPackageId = 5
                }
            );
    }
}
