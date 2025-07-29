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
                Latitude = 6.9271,
                Longitude = 79.8612
            },
            new LightPost
            {
                LightPostNumber = "LP002",
                Latitude = 6.9272,
                Longitude = 79.8613
            }
        );
    }
}
