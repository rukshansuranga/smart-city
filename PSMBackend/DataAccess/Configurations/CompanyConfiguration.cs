using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NodaTime;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasData(
            new Company
            {
                CompanyId = 1,
                Name = "Maga",
                AddressLine1 = "123 Main St",
                AddressLine2 = null,
                City = "Colombo",
                Mobile = "0771234567",
                CreatedAt = new LocalDateTime(2025, 6, 19, 14, 14, 0),
                UpdatedAt = null,
                CreatedBy = 1,
                UpdatedBy = null,
                IsActive = true
            },
            new Company
            {
                CompanyId = 2,
                Name = "Access Construction",
                AddressLine1 = "123 Main St",
                AddressLine2 = null,
                City = "Gampaha",
                Mobile = "0779876543",
                CreatedAt = new LocalDateTime(2025, 6, 19, 14, 14, 0),
                UpdatedAt = null,
                CreatedBy = 1,
                UpdatedBy = null,
                IsActive = true
            }
        );
    }
}
