using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NodaTime;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class LightPostComplainConfiguration : IEntityTypeConfiguration<LightPostComplint>
{
    public void Configure(EntityTypeBuilder<LightPostComplint> builder)
    {
        //feed light post complaint
        builder.HasData(
            new LightPostComplint
            {
                WorkPackageId = 2,
                Name = "lightisnotworking",
                Detail = "Light post LP001 is not working",
                CreatedDate = new LocalDateTime(2025, 6, 19, 14, 14),
                UpdatedDate = new LocalDateTime(2025, 6, 19, 14, 14),
                Status = "Open",
                ClientId = 1,
                LightPostNumber = "LP001"
            });

        builder.HasData(
        new LightPostComplint
        {
            WorkPackageId = 3,
            Name = "lightisnotworking",
            Detail = "Light post LP001 is not working 2",
            CreatedDate = new LocalDateTime(2025, 6, 19, 14, 14),
            UpdatedDate = new LocalDateTime(2025, 6, 19, 14, 14),
            Status = "Open",
            ClientId = 1,
            LightPostNumber = "LP001"
        });

        builder.HasData(
        new LightPostComplint
        {
            WorkPackageId = 4,
            Name = "lightisnotworking",
            Detail = "Light post LP001 is not working 3",
            CreatedDate = new LocalDateTime(2025, 6, 20, 14, 14),
            UpdatedDate = new LocalDateTime(2025, 6, 20, 14, 14),
            Status = "Open",
            ClientId = 1,
            LightPostNumber = "LP001"
        });

        builder.HasData(
        new LightPostComplint
        {
            WorkPackageId = 5,
            Name = "lightisnotworking",
            Detail = "Light post LP001 is not working 4",
            CreatedDate = new LocalDateTime(2025, 6, 20, 14, 14),
            UpdatedDate = new LocalDateTime(2025, 6, 20, 14, 14),
            Status = "Open",
            ClientId = 1,
            LightPostNumber = "LP002"
        });

        builder.HasData(
        new LightPostComplint
        {
            WorkPackageId = 6,
            Name = "lightisnotworking",
            Detail = "Light post LP001 is not working 5",
            CreatedDate = new LocalDateTime(2025, 6, 20, 14, 14),
            UpdatedDate = new LocalDateTime(2025, 6, 20, 14, 14),
            Status = "Open",
            ClientId = 1,
            LightPostNumber = "LP002"
        });
    }
}
