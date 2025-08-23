using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NodaTime;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasData(
            new Project
            {
                Id = 1,
                Subject = "Project Alpha",
                Description = "Description for Project Alpha",
                Type = ProjectType.Road,
                SpecificationDocument = "specification_alpha.pdf",
                StartDate = new LocalDate(2023, 1, 1),
                EndDate = new LocalDate(2023, 12, 31),
                Status = ProjectStatus.New,
                Location = "Location A",
                LocationNote = "Note for Location A",
                City = "Weliveriya",
                Latitude = 34.0522,
                Longitude = -118.2437,
                EstimatedCost = 1000000,
                TenderOpeningDate = new LocalDate(2023, 1, 15),
                TenderClosingDate = new LocalDate(2023, 1, 30),
                AwadedTenderId = 1,
                CreatedAt = new NodaTime.LocalDateTime(2023, 1, 1, 9, 0, 0),
                CreatedBy = "1",
                IsActive = true
            },
            new Project
            {
                Id = 2,
                Subject = "Weliveriya road project",
                Description = "Weliveriya road project",
                Type = ProjectType.Road,
                SpecificationDocument = "specification_alpha.pdf",
                StartDate = new LocalDate(2023, 1, 1 ),
                EndDate = new LocalDate(2023, 12, 31),
                Status = ProjectStatus.New,
                Location = "Location A",
                LocationNote = "Note for Location A",
                City = "Ambaraluwa",
                Latitude = 34.0522,
                Longitude = -118.2437,
                EstimatedCost = 1000000,
                TenderOpeningDate = new LocalDate(2023, 1, 15),
                TenderClosingDate = new LocalDate(2023, 1, 30),
                CreatedAt = new NodaTime.LocalDateTime(2023, 1, 1, 9, 0, 0),
                CreatedBy = "1",
                AwadedTenderId = 2,
                IsActive = true
            }
        );
    }
}
