using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NodaTime;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class DriverConfiguration : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.HasData(
            new Driver
            {
                UserId = "4",
                FirstName = "Upul",
                LastName = "Perera",
                Mobile = "0777582365",
                LicenseNo = "2345",
                ExpireDate = new LocalDateTime(2027,6,19,14,14),
                City = "Colombo",
                CreatedAt = new LocalDateTime(2025, 6, 19, 14, 14, 0),
                CreatedBy = "1",
                IsActive = true
            },
            new Driver
            {
                UserId = "5",
                FirstName = "Shantha",
                LastName = "Fernando",
                Mobile = "0147894492",
                LicenseNo = "2345",
                ExpireDate = new LocalDateTime(2028,6,19,14,14),
                City = "Galle",
                CreatedAt = new LocalDateTime(2025, 6, 19, 14, 14, 0),
                CreatedBy = "1",
                IsActive = true
            }
        );
    }
}
