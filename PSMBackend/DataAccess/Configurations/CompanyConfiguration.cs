using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasData(
            new Company
            {
                Id = 1,
                Name = "Maga",
                Address = "123 Main St, Colombo",
                
            },
            new Company
            {
                Id = 2,
                Name = "Access Construction",
                Address = "123 Main St, Gampaha",
            }
        );
    }
}
