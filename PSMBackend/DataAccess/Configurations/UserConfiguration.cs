using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
                //feed user
        builder.HasData(
            new User
            {
                UserId = 1,
                Name = "Admin",
                TelNumber = "0777582696"
            },
            new User
            {
                UserId = 2,
                Name = "Kamal",
                TelNumber = "0147894562"
            },
            new User
            {
                UserId = 3,
                Name = "Kumara",
                TelNumber = "0117654321"
            }
        );
    }
}
