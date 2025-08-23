using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NodaTime;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class TenderConfiguration : IEntityTypeConfiguration<Tender>
{
    public void Configure(EntityTypeBuilder<Tender> builder)
    {
        builder.HasData(
            new Tender
            {
                TenderId = 1,
                Subject = "Tender for Project A",
                Note = "This is a tender for Project A",
                SubmittedDate = new LocalDateTime(2023, 10, 1, 10, 0, 0),
                BidAmount = 500000,
                ProjectId = 1,
                CompanyId = 1,
                CreatedAt = new LocalDateTime(2023, 10, 1, 9, 0, 0),
                CreatedBy = "1",
                IsActive = true
            },
            new Tender
            {
                TenderId = 2,
                Subject = "Tender for Road Construction",
                Note = "This is a tender for Project A",
                SubmittedDate = new LocalDateTime(2023, 10, 1, 10, 0, 0),
                BidAmount = 300000,
                ProjectId = 1,
                CompanyId = 1,
                CreatedAt = new LocalDateTime(2023, 10, 1, 9, 0, 0),
                CreatedBy = "1",
                IsActive = true
            }
        );
    }
}
