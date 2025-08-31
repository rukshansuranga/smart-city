using System;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Npgsql;
using PSMDataAccess.Configurations;
using PSMModel.Models;

namespace PSMDataAccess;

public class ApplicationDbContext : DbContext
{
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<LightPost> LightPosts { get; set; }

    //Ticket
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<InternalTicket> InternalTickets { get; set; }
    public DbSet<ComplainTicket> ComplainTickets { get; set; }
    public DbSet<ProjectTicket> ProjectTickets { get; set; }
    public DbSet<TicketPackage> TicketPackages { get; set; }
    public DbSet<TicketHistory> TicketHistories { get; set; }
    public DbSet<TicketActivity> TicketActivities { get; set; }

    //Complains
    public DbSet<Complain> Complains { get; set; }
    public DbSet<LightPostComplain> LightPostComplains { get; set; }
    public DbSet<GeneralComplain> GeneralComplains { get; set; }
    public DbSet<ProjectComplain> ProjectComplains { get; set; }
    public DbSet<GarbageComplain> GarbageComplains { get; set; }

    
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<Vehical> Vehicals { get; set; }
    public DbSet<Route> Routes   { get; set; }
    public DbSet<RoutePoint> RoutePoints   { get; set; }
    public DbSet<Ride> Rides   { get; set; }
    public DbSet<RidePoint> RidePoints   { get; set; }
    public DbSet<GCShedule> GCShedules   { get; set; }
    public DbSet<Contractor> Contractors   { get; set; }
    public DbSet<Project> Projects   { get; set; }
    public DbSet<Tender> Tenders   { get; set; }
    public DbSet<ProjectInspector> ProjectInspectors { get; set; }
    public DbSet<ProjectInspection> ProjectInspections { get; set; }
    public DbSet<ProjectProgress> ProjectProgresses { get; set; }


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Complain>()
            .HasDiscriminator<string>("ComplainType")
            .HasValue<Complain>("Complain")
            .HasValue<LightPostComplain>("LightPostComplain")
            .HasValue<GarbageComplain>("GarbageComplain")
            .HasValue<GeneralComplain>("GeneralComplain")
            .HasValue<ProjectComplain>("ProjectComplain");

        modelBuilder.Entity<User>()
            .HasDiscriminator<string>("UserType")
            .HasValue<User>("User")
            .HasValue<Driver>("Driver");

        modelBuilder.Entity<Ticket>()
            .HasDiscriminator<string>("TicketType")
            .HasValue<ProjectTicket>("ProjectTicket")
            .HasValue<InternalTicket>("InternalTicket")
            .HasValue<ComplainTicket>("ComplainTicket");

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType).Property<LocalDateTime>("CreatedAt").HasDefaultValueSql("NOW()");
                modelBuilder.Entity(entityType.ClrType).Property<bool>("IsActive").HasDefaultValue(true);
            }
        }
        #region Configurations
        modelBuilder.ApplyConfiguration(new ClientConfiguration()); 
        modelBuilder.ApplyConfiguration(new LightPostConfiguration());
        //modelBuilder.ApplyConfiguration(new ComplainConfiguration());
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
        modelBuilder.ApplyConfiguration(new ContractorConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectConfiguration());
        modelBuilder.ApplyConfiguration(new TenderConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectComplainConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectInspectorConfiguration());

        #endregion
    }
}
