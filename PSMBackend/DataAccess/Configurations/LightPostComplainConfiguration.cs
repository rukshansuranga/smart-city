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
                WorkpackageId = 21,
                Subject = "Light Post Issue",
                Detail = "Light post not working at Main St.",
                Status = WorkpackageStatus.New,
                ClientId = "1",
                LightPostNumber = "LP001",
                CreatedAt = new LocalDateTime(2025, 6, 19, 14, 14, 0),
                CreatedBy = "1",
                IsActive = true
            },
            new LightPostComplain
            {
                WorkpackageId = 22,
                Subject = "Light Post Issue 2",
                Detail = "Light post LP001 is not working 2",
                Status = WorkpackageStatus.New,
                ClientId = "1",
                LightPostNumber = "LP001",
                CreatedAt = new LocalDateTime(2025, 6, 19, 14, 14, 0),
                CreatedBy = "1",
                IsActive = true
            },
            new LightPostComplain
            {
                WorkpackageId = 23,
                Subject = "Light Post Issue 3",
                Detail = "Light post LP001 is not working 3",
                Status = WorkpackageStatus.New,
                ClientId = "1",
                LightPostNumber = "LP001",
                CreatedAt = new LocalDateTime(2025, 6, 20, 14, 14, 0),
                CreatedBy = "1",
                IsActive = true
            },
            new LightPostComplain
            {
                WorkpackageId = 24,
                Subject = "Light Post Issue 4",
                Detail = "Light post LP002 is not working 4",
                Status = WorkpackageStatus.New,
                ClientId = "1",
                LightPostNumber = "LP002",
                CreatedAt = new LocalDateTime(2025, 6, 20, 14, 14, 0),
                CreatedBy = "1",
                IsActive = true
            },
            new LightPostComplain
            {
                WorkpackageId = 25,
                Subject = "Light Post Issue 5",
                Detail = "Light post LP002 is not working 5",
                Status = WorkpackageStatus.New,
                ClientId = "1",
                LightPostNumber = "LP002",
                CreatedAt = new LocalDateTime(2025, 6, 20, 14, 14, 0),
                CreatedBy = "1",
                IsActive = true
            }
        );
    }
}
