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
                ContractorId = "d0eca5fa-8cf4-4256-ab6a-9405c789c1b1",
                CreatedAt = new LocalDateTime(2023, 10, 1, 9, 0, 0),
                CreatedBy = "43e63068-a5fd-4a45-acfb-0383ff4d45ea",
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
                ContractorId = "d0eca5fa-8cf4-4256-ab6a-9405c789c1b1",
                CreatedAt = new LocalDateTime(2023, 10, 1, 9, 0, 0),
                CreatedBy = "43e63068-a5fd-4a45-acfb-0383ff4d45ea",
                IsActive = true
            },
            new Tender
            {
                TenderId = 3,
                Subject = "Tender for Project Building",
                Note = "This is a tender for Project 2",
                SubmittedDate = new LocalDateTime(2023, 10, 1, 10, 0, 0),
                BidAmount = 300000,
                ProjectId = 2,
                ContractorId = "a4aa5b28-36ab-4991-975a-5e5e441bf6fa",
                CreatedAt = new LocalDateTime(2023, 10, 1, 9, 0, 0),
                CreatedBy = "0c895075-b8e6-48f9-bb9e-2c9db9d7207a",
                IsActive = true
            },
            new Tender
            {
                TenderId = 4,
                Subject = "Tender for Road Building",
                Note = "This is a tender for Project 2",
                SubmittedDate = new LocalDateTime(2023, 10, 1, 10, 0, 0),
                BidAmount = 400000,
                ProjectId = 2,
                ContractorId = "a4aa5b28-36ab-4991-975a-5e5e441bf6fa",
                CreatedAt = new LocalDateTime(2023, 10, 1, 9, 0, 0),
                CreatedBy = "0c895075-b8e6-48f9-bb9e-2c9db9d7207a",
                IsActive = true
            }
        );
    }
}
