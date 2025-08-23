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
                UserId = "1",
                FirstName = "Admin",
                LastName = "User",
                Mobile = "0777582696",
                Email = "admin@example.com",
                City = "Colombo"
            },
            new User
            {
                UserId = "2",
                FirstName = "Kamal",
                LastName = "Perera",
                Mobile = "0147894562",
                Email = "kamal@example.com",
                City = "Galle"
            },
            new User
            {
                UserId = "3",
                FirstName = "Kumara",
                LastName = "Fernando",
                Mobile = "0117654321",
                Email = "kumara@example.com",
                City = "Kandy"
            }
        );
    }
}
