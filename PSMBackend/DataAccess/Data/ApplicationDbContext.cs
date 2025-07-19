using System;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Npgsql;
using PSMDataAccess.Configurations;
using PSMModel.Models;

namespace PSMDataAccess;

public class ApplicationDbContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<LightPost> LightPosts { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<WorkPackage> WorkPackages { get; set; }
    public DbSet<LightPostComplint> LightPostComplints { get; set; }

    public DbSet<TicketPackage> TicketPackages { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<GeneralComplain> GeneralComplains { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<Vehical> Vehicals { get; set; }
    public DbSet<Route> Routes   { get; set; }
    public DbSet<RoutePoint> RoutePoints   { get; set; }
    public DbSet<Ride> Rides   { get; set; }
    public DbSet<RidePoint> RidePoints   { get; set; }
    public DbSet<GCShedule> GCShedules   { get; set; }
    public DbSet<Company> Companies   { get; set; }
    public DbSet<Project> Projects   { get; set; }
    public DbSet<Tender> Tenders   { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     //optionsBuilder.UseNpgsql("Host=localhost;Database=OLD_PSM;Username=postgres;Password=test");

    //     optionsBuilder.UseNpgsql(
    //     "Host=localhost;Database=PSM2;Username=postgres;Password=test",
    //     o => o.UseNodaTime()
    // );
    // }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkPackage>()
            .HasDiscriminator<string>("WorkPackageType")
            .HasValue<WorkPackage>("WorkPackage")
            .HasValue<LightPostComplint>("LightPostComplint")
            .HasValue<GarbageComplain>("GarbageComplain")
            .HasValue<GeneralComplain>("GeneralComplain");

        modelBuilder.Entity<User>()
            .HasDiscriminator<string>("UserType")
            .HasValue<User>("User")
            .HasValue<Driver>("Driver");

        //modelBuilder.Entity<TicketPackage>().HasNoKey();

        #region FeedData

        //feed lightpost
        modelBuilder.Entity<LightPost>().HasData(
            new LightPost
            {
                LightPostNumber = "LP001",
                Latitude = 6.9271,
                Longitude = 79.8612
            },
            new LightPost
            {
                LightPostNumber = "LP002",
                Latitude = 6.9272,
                Longitude = 79.8613
            }
        );

        //feed garbage complain
        modelBuilder.Entity<GarbageComplain>().HasData(
            new GarbageComplain
            {
                WorkPackageId = 7,
                Name = "Garbage Collection",
                Detail = "Garbage collection needed in area",
                CreatedDate = new LocalDateTime(2025, 6, 19, 14, 14),
                UpdatedDate = new LocalDateTime(2025, 6, 19, 14, 14),
                Status = "Open",
                ClientId = 2,
                GarbagePointNo = "GP001"
            });

        modelBuilder.ApplyConfiguration(new ClientConfiguration());
        modelBuilder.ApplyConfiguration(new WorkpackageConfiguration());
        modelBuilder.ApplyConfiguration(new LightPostComplainConfiguration());
        modelBuilder.ApplyConfiguration(new TicketConfiguration());
        modelBuilder.ApplyConfiguration(new TicketPackageMappingConfiguration());
        modelBuilder.ApplyConfiguration(new GeneralComplainConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new VehicalConfiguration());
        modelBuilder.ApplyConfiguration(new DriverConfiguration());
        modelBuilder.ApplyConfiguration(new RouteConfiguration());
        modelBuilder.ApplyConfiguration(new RoutePointConfiguration());
        modelBuilder.ApplyConfiguration(new RideConfiguration());
        modelBuilder.ApplyConfiguration(new RegionConfiguration());
        modelBuilder.ApplyConfiguration(new GCSheduleConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectConfiguration());
        modelBuilder.ApplyConfiguration(new TenderConfiguration());

        #endregion
    }
}
