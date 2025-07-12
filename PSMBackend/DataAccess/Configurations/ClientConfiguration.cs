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
                ClientId = 1,
                Name = "Jeewan",
                Mobile = "0714789562"
            },
            new Client
            {
                ClientId = 2,
                Name = "Lakshitha",
                Mobile = "07777894562"
            }
        );
    }
}
