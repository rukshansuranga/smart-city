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
                UserId = 4,
                Name = "Upul",
                TelNumber = "0777582365",
                LicenNo = "2345",
                ExpireDate = new LocalDateTime(2027,6,19,14,14),
            },
            new Driver
            {
                UserId = 5,
                Name = "Shantha",
                TelNumber = "0147894492",
                LicenNo = "2345",
                ExpireDate = new LocalDateTime(2028,6,19,14,14),
            }
        );
    }
}
