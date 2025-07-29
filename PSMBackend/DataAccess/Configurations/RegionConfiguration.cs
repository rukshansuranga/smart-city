using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class RegionConfiguration : IEntityTypeConfiguration<Region>
{
    public void Configure(EntityTypeBuilder<Region> builder)
    {
        builder.HasData(
            new Region
            {
                RegionNo = "R001",
                Name = "Weliveriya South",
                Note = "This is a region in Gampaha district.",
                CreatedAt = new NodaTime.LocalDateTime(2023, 1, 1, 9, 0, 0),
                CreatedBy = 1,
                IsActive = true
            },
            new Region
            {
                RegionNo = "R002",
                Name = "Weliveriya North",
                Note = "This is a region in Gampaha district.",
                CreatedAt = new NodaTime.LocalDateTime(2023, 1, 1, 9, 0, 0),
                CreatedBy = 1,
                IsActive = true
            }
        );
    }
}
