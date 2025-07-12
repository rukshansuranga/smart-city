using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class GCSheduleConfiguration : IEntityTypeConfiguration<GCShedule>
{
    public void Configure(EntityTypeBuilder<GCShedule> builder)
    {
        builder.HasData(
            //Region R001
            new GCShedule
            {
                Id = 1,
                Day = "Monday",
                Type = "Platic",
                Time = "Morning",
                RegionNo = "R001",
            },
            new GCShedule
            {
                Id = 2,
                Day = "Tuesday",
                Type = "Garbage",
                Time = "Morning",
                RegionNo = "R001",
            },
            new GCShedule
            {
                Id = 3,
                Day = "Wednesday",
                Type = "Garbage",
                Time = "Morning",
                RegionNo = "R001",
            },
            new GCShedule
            {
                Id = 4,
                Day = "Thursday",
                Type = "Garbage",
                Time = "Afternoon",
                RegionNo = "R001",
            },
            new GCShedule
            {
                Id = 5,
                Day = "Friday",
                Type = "Paper",
                Time = "Morning",
                RegionNo = "R001",
            },
            //Region R002
            new GCShedule
            {
                Id = 6,
                Day = "Monday",
                Type = "Platic",
                Time = "Morning",
                RegionNo = "R001",
            },
            new GCShedule
            {
                Id = 7,
                Day = "Tuesday",
                Type = "Garbage",
                Time = "Morning",
                RegionNo = "R002",
            },
            new GCShedule
            {
                Id = 8,
                Day = "Wednesday",
                Type = "Garbage",
                Time = "Morning",
                RegionNo = "R002",
            },
            new GCShedule
            {
                Id = 9,
                Day = "Thursday",
                Type = "Garbage",
                Time = "Afternoon",
                RegionNo = "R002",
            },
            new GCShedule
            {
                Id = 10,
                Day = "Friday",
                Type = "Paper",
                Time = "Morning",
                RegionNo = "R002",
            }
        );
    }
}
