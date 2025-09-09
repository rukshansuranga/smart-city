using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class LightPostConfiguration : IEntityTypeConfiguration<LightPost>
{
    public void Configure(EntityTypeBuilder<LightPost> builder)
    {
       
        
        // Seed data for LightPost
        builder.HasData(
            new  LightPost
            {
                LightPostNumber = "LP001",
                Latitude = 7.025934,
                Longitude = 80.026858
            },
            new LightPost
            {
                LightPostNumber = "LP002",
                Latitude = 7.027679,
                Longitude = 80.027621
            },
            new LightPost
            {
                LightPostNumber = "LP003",
                Latitude = 7.030833,
                Longitude = 80.028532
            },
            new LightPost
            {
                LightPostNumber = "LP004",
                Latitude = 7.033020,
                Longitude = 80.027282

            },
            new LightPost
            {
                LightPostNumber = "LP005",
                Latitude = 7.034828,
                Longitude = 80.025926
            }
        );
    }
}




