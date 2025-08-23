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
                Name = "Noth Root",
                CreatedAt = new NodaTime.LocalDateTime(2023, 1, 1, 9, 0, 0),
                CreatedBy = "1",
                IsActive = true
            },
            new Route
            {
                RouteNo = "R002",
                Name = "South Root",
                CreatedAt = new NodaTime.LocalDateTime(2023, 1, 1, 9, 0, 0),
                CreatedBy = "1",
                IsActive = true
            }
        );
    }
}
