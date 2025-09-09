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
                ComplainId = 4,
                Subject = "Project Complain 1",
                Detail = "Project Complain 1 description",
                Status = ComplainStatus.New,
                ClientId = "1",
                ProjectId = 1,
                CreatedAt = new LocalDateTime(2025, 6, 19, 14, 14, 0),
                CreatedBy = "1",
                IsActive = true
            },
            new ProjectComplain
            {
                ComplainId = 5,
                Subject = "Project Complain 2",
                Detail = "Project Complain 2 description",
                Status = ComplainStatus.InProgress,
                ClientId = "2",
                ProjectId = 2,
                CreatedAt = new LocalDateTime(2025, 6, 20, 10, 0, 0),
                CreatedBy = "2",
                IsActive = true
            });
    }
}
