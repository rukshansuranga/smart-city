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
                Id = 1,
                Name = "maduma bankara vidyalaya",
                Longitude=7.024998,
                Latitude=80.026489,
                RouteNo="R001"
            },
            new RoutePoint
            {
                Id = 2  ,
                Name = "gala gawa",
                Longitude=7.025465,
                Latitude=80.022924,
                RouteNo="R001"
            },
            new RoutePoint
            {
                Id = 3,
                Name = "Sewa Piyasa",
                Longitude=7.022591,
                Latitude=80.021614,
                RouteNo="R001"
            },
            new RoutePoint
            {
                Id = 4,
                Name = "jayamal oil",
                Longitude=7.022879,
                Latitude=80.023322,
                RouteNo="R001"
            },
            new RoutePoint
            {
                Id = 5,
                Name = "bathiya home",
                Longitude=7.021252,
                Latitude=80.026512,
                RouteNo="R001"
            },
            new RoutePoint
            {
                Id = 6,
                Name = "grace health",
                Longitude=7.022968,
                Latitude=80.026774,
                RouteNo="R001"
            }
        );
    }
}
