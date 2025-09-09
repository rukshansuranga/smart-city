using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class RoutePointConfiguration : IEntityTypeConfiguration<RoutePoint>
{
    public void Configure(EntityTypeBuilder<RoutePoint> builder)
    {
        builder.HasData(
            new RoutePoint
            {
                RoutePointId = 1,
                PointName = "maduma bankara vidyalaya",
                Longitude = 7.024998,
                Latitude = 80.026489,
                RouteNo = "R001",
                CreatedAt = new NodaTime.LocalDateTime(2023, 1, 1, 9, 0, 0),
                CreatedBy = "1",
                IsActive = true
            },
            new RoutePoint
            {
                RoutePointId = 2,
                PointName = "gala gawa",
                Longitude = 7.025465,
                Latitude = 80.022924,
                RouteNo = "R001",
                CreatedAt = new NodaTime.LocalDateTime(2023, 1, 1, 9, 0, 0),
                CreatedBy = "1",
                IsActive = true
            },
            new RoutePoint
            {
                RoutePointId = 3,
                PointName = "Sewa Piyasa",
                Longitude = 7.022591,
                Latitude = 80.021614,
                RouteNo = "R001",
                CreatedAt = new NodaTime.LocalDateTime(2023, 1, 1, 9, 0, 0),
                CreatedBy = "1",
                IsActive = true
            },
            new RoutePoint
            {
                RoutePointId = 4,
                PointName = "jayamal oil",
                Longitude = 7.022879,
                Latitude = 80.023322,
                RouteNo = "R001",
                CreatedAt = new NodaTime.LocalDateTime(2023, 1, 1, 9, 0, 0),
                CreatedBy = "1",
                IsActive = true
            },
            new RoutePoint
            {
                RoutePointId = 5,
                PointName = "bathiya home",
                Longitude = 7.021252,
                Latitude = 80.026512,
                RouteNo = "R001",
                CreatedAt = new NodaTime.LocalDateTime(2023, 1, 1, 9, 0, 0),
                CreatedBy = "1",
                IsActive = true
            },
            new RoutePoint
            {
                RoutePointId = 6,
                PointName = "grace health",
                Longitude = 7.022968,
                Latitude = 80.026774,
                RouteNo = "R001",
                CreatedAt = new NodaTime.LocalDateTime(2023, 1, 1, 9, 0, 0),
                CreatedBy = "1",
                IsActive = true
            }
        );
    }
}
