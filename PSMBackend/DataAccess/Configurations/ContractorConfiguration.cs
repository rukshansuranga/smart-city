using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NodaTime;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class ContractorConfiguration : IEntityTypeConfiguration<Contractor>
{
    public void Configure(EntityTypeBuilder<Contractor> builder)
    {
        builder.HasData(
            new Contractor
            {
                ContractorId = "d0eca5fa-8cf4-4256-ab6a-9405c789c1b1",
                Name = "Maga",
                AddressLine1 = "123 Main St",
                AddressLine2 = null,
                City = "Colombo",
                Mobile = "0771234567",
                CreatedAt = new LocalDateTime(2025, 6, 19, 14, 14, 0),
                UpdatedAt = null,
                CreatedBy = "1",
                UpdatedBy = null,
                IsActive = true
            },
            new Contractor
            {
                ContractorId = "a4aa5b28-36ab-4991-975a-5e5e441bf6fa",
                Name = "Access Construction",
                AddressLine1 = "123 Main St",
                AddressLine2 = null,
                City = "Gampaha",
                Mobile = "0779876543",
                CreatedAt = new LocalDateTime(2025, 6, 19, 14, 14, 0),
                UpdatedAt = null,
                CreatedBy = "1",
                UpdatedBy = null,
                IsActive = true
            }
        );
    }
}
