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
                UserId = "0c895075-b8e6-48f9-bb9e-2c9db9d7207a",
                FirstName = "Ajith",
                LastName = "User",
                Mobile = "0777582696",
                Email = "ajith@example.com",
                City = "Colombo",
                Council = "Mahara"
            },
            new User
            {
                UserId = "43e63068-a5fd-4a45-acfb-0383ff4d45ea",
                FirstName = "Kumara",
                LastName = "Perera",
                Mobile = "0147894562",
                Email = "kumara@example.com",
                City = "Galle",
                Council = "Mahara"
            },
            new User
            {
                UserId = "6c35c5ad-2f70-4c3f-aa44-c94bc61d10a1",
                FirstName = "Upul",
                LastName = "Fernando",
                Mobile = "0117654321",
                Email = "upul@example.com",
                City = "Kandy",
                Council = "Mahara"
            },
            new User
            {
                UserId = "d0eca5fa-8cf4-4256-ab6a-9405c789c1b1",
                FirstName = "kamal",
                LastName = "Fernando",
                Mobile = "0117654321",
                Email = "kamal@example.com",
                City = "Kandy",
                Council = "Mahara"
            },
            new User
            {
                UserId = "a4aa5b28-36ab-4991-975a-5e5e441bf6fa",
                FirstName = "constractor2",
                LastName = "Fernando",
                Mobile = "0117654321",
                Email = "kamal@example.com",
                City = "Kandy",
                Council = "Mahara"
            }
        );
    }
}
