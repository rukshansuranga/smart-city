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
            },
            new Region
            {
                RegionNo = "R002",
                Name = "Weliveriya North",
                Note = "This is a region in Gampaha district.",
            }
        );
    }
}
