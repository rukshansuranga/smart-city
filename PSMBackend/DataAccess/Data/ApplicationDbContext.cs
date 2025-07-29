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
    public DbSet<Workpackage> Workpackages { get; set; }
    public DbSet<LightPostComplain> LightPostComplains { get; set; }

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
    public DbSet<ProjectComplain> ProjectComplains { get; set; }
    public DbSet<TicketHistory> TicketHistories { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }


    //local connection
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     //optionsBuilder.UseNpgsql("Host=localhost;Database=OLD_PSM;Username=postgres;Password=test");

    //     optionsBuilder.UseNpgsql(
    //     "Host=localhost;Database=PSM2;Username=postgres;Password=test",
    //     o => o.UseNodaTime()
    // );
    // }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     //optionsBuilder.UseNpgsql("Host=localhost;Database=OLD_PSM;Username=postgres;Password=test");

    //     optionsBuilder.UseNpgsql(
    //     "Host=ep-morning-math-a10tj24y-pooler.ap-southeast-1.aws.neon.tech; Database=neondb; Username=neondb_owner; Password=npg_DiA1HhTn2Gmr; SSL Mode=VerifyFull; Channel Binding=Require;",
    //     o => o.UseNodaTime()
    // );
    // }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Workpackage>()
            .HasDiscriminator<string>("WorkpackageType")
            .HasValue<Workpackage>("Workpackage")
            .HasValue<LightPostComplain>("LightPostComplain")
            .HasValue<GarbageComplain>("GarbageComplain")
            .HasValue<GeneralComplain>("GeneralComplain")
            .HasValue<ProjectComplain>("ProjectComplain");

        modelBuilder.Entity<User>()
            .HasDiscriminator<string>("UserType")
            .HasValue<User>("User")
            .HasValue<Driver>("Driver");

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType).Property<LocalDateTime>("CreatedDate").HasDefaultValueSql("NOW()");
                modelBuilder.Entity(entityType.ClrType).Property<bool>("IsActive").HasDefaultValue(true);
            }
        }
        #region Configurations
        modelBuilder.ApplyConfiguration(new ClientConfiguration()); 
        modelBuilder.ApplyConfiguration(new LightPostConfiguration());
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
        modelBuilder.ApplyConfiguration(new ProjectComplainConfiguration());

        #endregion
    }
}
