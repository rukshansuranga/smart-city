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
                WorkPackageId = 12,
                Name = "General Complain 1",
                Detail = "General Complain 1 description",
                CreatedDate = new LocalDateTime(2025, 6, 19, 14, 14),
                UpdatedDate = new LocalDateTime(2025, 6, 19, 14, 14),
                Status = "New",
                ClientId = 1,
                IsPrivate = false,
            });
    }
}
