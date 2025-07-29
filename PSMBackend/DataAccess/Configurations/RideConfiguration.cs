using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NodaTime;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class RideConfiguration : IEntityTypeConfiguration<Ride>
{
    public void Configure(EntityTypeBuilder<Ride> builder)
    {
        builder.HasData(
            new Ride
            {
                RideId = 1,
                StartTime = new LocalDateTime(2025, 7, 10, 13, 0, 0),
                EndTime = null,
                Type = "Paper Collection",
                Notes = null,
                DriverNo = 1,
                VehicalNo = "T01",
                RegionNo = "R001",
                CreatedAt = new LocalDateTime(2025, 7, 10, 12, 0, 0),
                CreatedBy = 1,
                IsActive = true
            }
        );
    }
}
