using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NodaTime;
using PSMModel.Models;
using PSMModel.Enums;

namespace PSMDataAccess.Configurations;

public class LightPostComplainConfiguration : IEntityTypeConfiguration<LightPostComplain>
{
    public void Configure(EntityTypeBuilder<LightPostComplain> builder)
    {
        builder.HasData(
            new LightPostComplain
            {
                ComplainId = 2,
                Subject = "Light Post Issue",
                Detail = "Light post not working at Main St.",
                Status = ComplainStatus.New,
                ClientId = "1",
                LightPostNumber = "LP001",
                CreatedAt = new LocalDateTime(2025, 6, 19, 14, 14, 0),
                CreatedBy = "1",
                IsActive = true
            },
            new LightPostComplain
            {
                ComplainId = 3,
                Subject = "Light Post Issue 2",
                Detail = "Light post LP001 is not working 2",
                Status = ComplainStatus.New,
                ClientId = "1",
                LightPostNumber = "LP001",
                CreatedAt = new LocalDateTime(2025, 6, 19, 14, 14, 0),
                CreatedBy = "1",
                IsActive = true
            }
            
        );
    }
}
