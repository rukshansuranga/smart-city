using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class VehicalConfiguration : IEntityTypeConfiguration<Vehical>
{

public void Configure(EntityTypeBuilder<Vehical> builder)
    {
        builder.HasData(
            new Vehical
            {
                VehicalNo = "T01",
                Brand = "John Dear",
                Model = "T66",
                RegistrationNo = "GA1234",
                Year="1998"
            },
            new Vehical
            {
                VehicalNo = "T02",
                Brand = "Toyota",
                Model = "Truch",
                RegistrationNo = "CAG3456",
                Year="2024"
            }
        );
    }
}
