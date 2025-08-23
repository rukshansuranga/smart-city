using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NodaTime;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class GeneralComplainConfiguration : IEntityTypeConfiguration<GeneralComplain>
{
    public void Configure(EntityTypeBuilder<GeneralComplain> builder)
    {
         //feed light general complain
        builder.HasData(
            new GeneralComplain
            {
                WorkpackageId = 12,
                Subject = "General Complain 1",
                Detail = "General Complain 1 description",
                Status = PSMModel.Enums.WorkpackageStatus.New,
                ClientId = "1",
                IsPrivate = false,
                CreatedAt = new NodaTime.LocalDateTime(2025, 6, 19, 14, 14, 0),
                CreatedBy = "1",
                IsActive = true
            });
    }
}
