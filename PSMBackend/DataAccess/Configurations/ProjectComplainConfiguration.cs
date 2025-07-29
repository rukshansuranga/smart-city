using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NodaTime;
using PSMModel.Models;
using PSMModel.Enums;

namespace PSMDataAccess.Configurations;

public class ProjectComplainConfiguration : IEntityTypeConfiguration<ProjectComplain>
{
    public void Configure(EntityTypeBuilder<ProjectComplain> builder)
    {
        builder.HasData(
            new ProjectComplain
            {
                WorkpackageId = 31,
                Subject = "Project Complain 1",
                Detail = "Project Complain 1 description",
                Status = WorkpackageStatus.New,
                ClientId = 1,
                ProjectId = 1,
                CreatedAt = new LocalDateTime(2025, 6, 19, 14, 14, 0),
                CreatedBy = 1,
                IsActive = true
            },
            new ProjectComplain
            {
                WorkpackageId = 32,
                Subject = "Project Complain 2",
                Detail = "Project Complain 2 description",
                Status = WorkpackageStatus.InProgress,
                ClientId = 2,
                ProjectId = 2,
                CreatedAt = new LocalDateTime(2025, 6, 20, 10, 0, 0),
                CreatedBy = 2,
                IsActive = true
            }
            // new ProjectComplain
            // {
            //     WorkpackageId = 33,
            //     Subject = "Project Complain 3",
            //     Detail = "Project Complain 3 description",
            //     Status = WorkpackageStatus.Close,
            //     ClientId = 2,
            //     ProjectId = 2,
            //     CreatedAt = new LocalDateTime(2025, 6, 21, 9, 30, 0),
            //     CreatedBy = 2,
            //     IsActive = true
            );
    }
}
