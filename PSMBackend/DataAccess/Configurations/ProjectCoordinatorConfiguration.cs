using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NodaTime;
using PSMModel.Models;
using PSMModel.Enums;

namespace PSMDataAccess.Configurations;

public class ProjectCoordinatorConfiguration : IEntityTypeConfiguration<ProjectCoordinator>
{
    public void Configure(EntityTypeBuilder<ProjectCoordinator> builder)
    {
        builder.HasData(
            new ProjectCoordinator
            {
                ProjectCoordinatorId = 1,
                ProjectId = 1,
                CoordinatorId = "0c895075-b8e6-48f9-bb9e-2c9db9d7207a",
                AssignDate = new LocalDate(2025, 8, 24),
                CoordinatorType = ProjectCoordinatorType.Coordinator,
                Note = "Can you provide an update on the project status?"
            },
            new ProjectCoordinator
            {
                ProjectCoordinatorId = 2,
                ProjectId = 1,
                CoordinatorId = "43e63068-a5fd-4a45-acfb-0383ff4d45ea",
                AssignDate = new LocalDate(2025, 8, 24),
                CoordinatorType = ProjectCoordinatorType.Supporter,
                Note = "Can you give support on the project?"
            },
            new ProjectCoordinator
            {
                ProjectCoordinatorId = 3,
                ProjectId = 2,
                CoordinatorId = "6c35c5ad-2f70-4c3f-aa44-c94bc61d10a1",
                AssignDate = new LocalDate(2025, 8, 24),
                CoordinatorType = ProjectCoordinatorType.Coordinator,
                Note = "Can you give support on the project?"
            }
        );
    }
}
