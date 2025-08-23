using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        //feed client
        builder.HasData(
            new Client
            {
                ClientId = "1",
                AddressLine1 = "123 Main St",
                AddressLine2 = "Apt 4B",
                City = "Colombo",
                CreatedAt = new NodaTime.LocalDateTime(2025, 6, 19, 14, 14, 0),
                CreatedBy = "1",
                IsActive = true
            },
            new Client
            {
                ClientId = "2",
                AddressLine1 = "456 Lake Rd",
                AddressLine2 = null,
                City = "Kandy",
                CreatedAt = new NodaTime.LocalDateTime(2025, 6, 19, 14, 14, 0),
                CreatedBy = "1",
                IsActive = true
            }
        );
    }
}
