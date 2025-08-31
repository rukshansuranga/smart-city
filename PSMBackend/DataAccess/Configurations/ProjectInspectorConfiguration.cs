using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NodaTime;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class ProjectInspectorConfiguration : IEntityTypeConfiguration<ProjectInspector>
{
    public void Configure(EntityTypeBuilder<ProjectInspector> builder)
    {
        builder.HasData(
            new ProjectInspector
            {
                ProjectInspectorId = 1,
                ProjectId = 1,
                InspectorId = "0c895075-b8e6-48f9-bb9e-2c9db9d7207a",
                AssignDate = new LocalDate(2025, 8, 24),
                Responsibility = "Inspect the project",
                Note = "Can you provide an update on the project status?"
            },
            new ProjectInspector
            {
                ProjectInspectorId = 2,
                ProjectId = 1,
                InspectorId = "43e63068-a5fd-4a45-acfb-0383ff4d45ea",
                AssignDate = new LocalDate(2025, 8, 24),
                Responsibility = "Support",
                Note = "Can you give support on the project?"
            },
            new ProjectInspector
            {
                ProjectInspectorId = 3,
                ProjectId = 2,
                InspectorId = "6c35c5ad-2f70-4c3f-aa44-c94bc61d10a1",
                AssignDate = new LocalDate(2025, 8, 24),
                Responsibility = "Manage",
                Note = "Can you give support on the project?"
            }
        );
    }
}
