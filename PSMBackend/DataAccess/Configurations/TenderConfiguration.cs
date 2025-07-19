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
                Id = 1,
                Name = "Tender for Project A",
                Note = "This is a tender for Project A",
                SubmittedDate = new LocalDateTime(2023, 10, 1, 10, 0),
                BidAmount = 500000,
                ProjectId = 1,
                CompanyId = 1
            },
            new Tender
            {
                Id = 2,
                Name = "Tender for Road Construction",
                Note = "This is a tender for Project A",
                SubmittedDate = new LocalDateTime(2023, 10, 1, 10, 0),
                BidAmount = 300000,
                ProjectId = 1,
                CompanyId = 1
            }
        );
    }
}
