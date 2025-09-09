using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NodaTime;
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
                GCSheduleId = 1,
                Day = "Monday",
                Type = "Plastic",
                Time = "Morning",
                RegionNo = "R001",
            },
            new GCShedule
            {
                GCSheduleId = 2,
                Day = "Tuesday",
                Type = "Garbage",
                Time = "Morning",
                RegionNo = "R001",
            },
            new GCShedule
            {
                GCSheduleId = 3,
                Day = "Wednesday",
                Type = "Garbage",
                Time = "Morning",
                RegionNo = "R001",
            },
            new GCShedule
            {
                GCSheduleId = 4,
                Day = "Thursday",
                Type = "Garbage",
                Time = "Afternoon",
                RegionNo = "R001",
            },
            new GCShedule
            {
                GCSheduleId = 5,
                Day = "Friday",
                Type = "Paper",
                Time = "Morning",
                RegionNo = "R001",
            },
            //Region R002
            new GCShedule
            {
                GCSheduleId = 6,
                Day = "Monday",
                Type = "Plastic",
                Time = "Morning",
                RegionNo = "R002",
            },
            new GCShedule
            {
                GCSheduleId = 7,
                Day = "Tuesday",
                Type = "Garbage",
                Time = "Morning",
                RegionNo = "R002",
            },
            new GCShedule
            {
                GCSheduleId = 8,
                Day = "Wednesday",
                Type = "Garbage",
                Time = "Morning",
                RegionNo = "R002",
            },
            new GCShedule
            {
                GCSheduleId = 9,
                Day = "Thursday",
                Type = "Garbage",
                Time = "Afternoon",
                RegionNo = "R002",
            },
            new GCShedule
            {
                GCSheduleId = 10,
                Day = "Friday",
                Type = "Paper",
                Time = "Morning",
                RegionNo = "R002",
            }
        );
    }
}
