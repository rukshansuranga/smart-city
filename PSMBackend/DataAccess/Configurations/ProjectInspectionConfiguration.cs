using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NodaTime;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class ProjectInspectionConfiguration : IEntityTypeConfiguration<ProjectInspection>
{
    public void Configure(EntityTypeBuilder<ProjectInspection> builder)
    {
        builder.HasData(
            new ProjectInspection
            {
                ProjectInspectionId = 1,
                ProjectInspectorId = 1,
                Findings = "No major issues found.",
                Recommendations = "Continue with the current plan.",
                CreatedAt = new LocalDateTime(2023, 10, 1, 9, 0, 0),
                CreatedBy = "43e63068-a5fd-4a45-acfb-0383ff4d45ea",
                IsActive = true
            },
            new ProjectInspection
            {
                ProjectInspectionId = 2,
                ProjectInspectorId = 1,
                Findings = "Minor issues found.",
                Recommendations = "Address the minor issues.",
                CreatedAt = new LocalDateTime(2023, 10, 8, 9, 0, 0),
                CreatedBy = "43e63068-a5fd-4a45-acfb-0383ff4d45ea",
                IsActive = true
            }
           
        );
    }
}
