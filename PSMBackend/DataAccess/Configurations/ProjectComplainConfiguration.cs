using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NodaTime;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class ProjectComplainConfiguration : IEntityTypeConfiguration<ProjectComplain>
{
    public void Configure(EntityTypeBuilder<ProjectComplain> builder)
    {
        builder.HasData(
            new ProjectComplain
            {
                WorkPackageId = 31,
                Name = "Project Complain 1",
                Detail = "Project Complain 1 description",
                CreatedDate = new LocalDateTime(2025, 6, 19, 14, 14),
                UpdatedDate = new LocalDateTime(2025, 6, 19, 14, 14),
                Status = "New",
                ClientId = 1,
                ProjectId = 1
            },
            new ProjectComplain
            {
                WorkPackageId = 32,
                Name = "Project Complain 2",
                Detail = "Project Complain 2 description",
                CreatedDate = new LocalDateTime(2025, 6, 20, 10, 0),
                UpdatedDate = new LocalDateTime(2025, 6, 20, 10, 0),
                Status = "In Progress",
                ClientId = 2,
                ProjectId = 2
            },
            new ProjectComplain
            {
                WorkPackageId = 33,
                Name = "Project Complain 3",
                Detail = "Project Complain 3 description",
                CreatedDate = new LocalDateTime(2025, 6, 21, 9, 30),
                UpdatedDate = new LocalDateTime(2025, 6, 21, 9, 30),
                Status = "Resolved",
                ClientId = 2,
                ProjectId = 3
            });
    }
}
