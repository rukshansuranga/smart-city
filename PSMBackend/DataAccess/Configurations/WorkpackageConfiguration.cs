using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NodaTime;
using PSMModel.Enums;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class WorkpackageConfiguration : IEntityTypeConfiguration<Workpackage>
{
    public void Configure(EntityTypeBuilder<Workpackage> builder)
    {
        //feed workpackage
        builder.HasData(
            new Workpackage
            {
                WorkpackageId = 1,
                Subject = "Work Package 1",
                Detail = "Detail for Work Package 1",
                Status = WorkpackageStatus.New,
                ClientId = "2",
                WorkpackageType = "Workpackage",
                CreatedAt = new LocalDateTime(2025, 6, 19, 14, 14, 0),
                UpdatedAt = new LocalDateTime(2025, 6, 19, 14, 14, 0),
                CreatedBy = "1",
                IsActive = true
            },
            new Workpackage
            {
                WorkpackageId = 2,
                Subject = "Work Package 2",
                Detail = "Detail for Work Package 2",
                Status = WorkpackageStatus.InProgress,
                ClientId = "2",
                WorkpackageType = "Workpackage",
                CreatedAt = new LocalDateTime(2025, 7, 1, 10, 0, 0),
                UpdatedAt = new LocalDateTime(2025, 7, 1, 10, 0, 0),
                CreatedBy = "2",
                IsActive = true
            },
            new Workpackage
            {
                WorkpackageId = 3,
                Subject = "Work Package 3",
                Detail = "Detail for Work Package 3",
                Status = WorkpackageStatus.InProgress,
                ClientId = "2",
                WorkpackageType = "Workpackage",
                CreatedAt = new LocalDateTime(2025, 7, 10, 9, 30, 0),
                UpdatedAt = new LocalDateTime(2025, 7, 10, 9, 30, 0),
                CreatedBy = "3",
                IsActive = true
            },
            new Workpackage
            {
                WorkpackageId = 4,
                Subject = "Work Package 4",
                Detail = "Detail for Work Package 4",
                Status = WorkpackageStatus.New,
                ClientId = "2",
                WorkpackageType = "Workpackage",
                CreatedAt = new LocalDateTime(2025, 7, 15, 8, 0, 0),
                UpdatedAt = new LocalDateTime(2025, 7, 15, 8, 0, 0),
                CreatedBy = "4",
                IsActive = true
            },
            new Workpackage
            {
                WorkpackageId = 5,
                Subject = "Work Package 5",
                Detail = "Detail for Work Package 5",
                Status = WorkpackageStatus.New,
                ClientId = "2",
                WorkpackageType = "Workpackage",
                CreatedAt = new LocalDateTime(2025, 7, 20, 11, 45, 0),
                UpdatedAt = new LocalDateTime(2025, 7, 20, 11, 45, 0),
                CreatedBy = "1",
                IsActive = true
            }
        );


    }
}
