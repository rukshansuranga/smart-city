using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class RouteConfiguration : IEntityTypeConfiguration<Route>
{
    public void Configure(EntityTypeBuilder<Route> builder)
    {
        builder.HasData(
            new Route
            {
                RouteNo = "R001",
                Name="Noth Root"
            },
            new Route
            {
                RouteNo = "R002",
                Name="South Root"
            }
        );
    }
}
