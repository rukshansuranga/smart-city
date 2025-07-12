using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NodaTime;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class WorkpackageConfiguration : IEntityTypeConfiguration<WorkPackage>
{
    public void Configure(EntityTypeBuilder<WorkPackage> builder)
    {
        //feed workpackage
        builder.HasData(
            new WorkPackage
            {
                WorkPackageId = 1,
                Name = "Garbage Collection",
                Detail = "Garbage collection needed in area LP002",
                CreatedDate = new LocalDateTime(2025,6,19,14,14),
                UpdatedDate = new LocalDateTime(2025,6,19,14,14),
                Status = "Open",
                ClientId = 2
            }
        );


    }
}
