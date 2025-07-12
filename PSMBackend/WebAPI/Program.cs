using System.Net;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using PSMDataAccess;
using PSMWebAPI.Repositories;
using PSMWebAPI.Services;

var builder = WebApplication.CreateBuilder(args);



// builder.Host.ConfigureServices((context, services) =>
// {
//     HostConfig.CertPath = context.Configuration["CertPath"];
//     HostConfig.CertPassword = context.Configuration["CertPassword"];
// });

// builder.Host.ConfigureWebHostDefaults(webBuilder =>
// {
//     webBuilder.ConfigureKestrel(opt =>
//     {
//         opt.ListenAnyIP(5000, listOpt =>
//         {
//             listOpt.UseHttps(HostConfig.CertPath, HostConfig.CertPassword);
//         });
//     });
// });

// builder.WebHost.UseKestrel(opt =>
// {
//     opt.ListenAnyIP(5000, listOpt =>
//     {
//         listOpt.UseHttps(builder.Configuration["CertPath"],  builder.Configuration["CertPassword"]);
//     });
// });

// builder.WebHost.ConfigureKestrel((context, serverOptions) =>
// {
//     serverOptions.Listen(IPAddress.Loopback, 5000, listenOptions =>
//     {
//         listenOptions.UseHttps(builder.Configuration["CertPath"], builder.Configuration["CertPassword"]);
//     });
// });

builder.Services.AddControllers().AddJsonOptions(options =>
   options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddCors();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), o => o.UseNodaTime()));

// Register repositories and services
builder.Services.AddScoped<IWorkpackageRepository, WorkpackageRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IGarbageRepository, GarbageRepository>();

//builder.Services.AddHostedService<RideSimulationService>();

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply migrations automatically
ApplyMigrations(app);

app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast =  Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast")
// .WithOpenApi();

app.Run();

static void ApplyMigrations(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Check and apply pending migrations
                var pendingMigrations = dbContext.Database.GetPendingMigrations();
                if (pendingMigrations.Any())
                {
                    Console.WriteLine("Applying pending migrations...");
                    dbContext.Database.Migrate();
                    Console.WriteLine("Migrations applied successfully.");
                }
                else
                {
                    Console.WriteLine("No pending migrations found.");
                }
            }
        }

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public static class HostConfig
{
    public static string CertPath { get; set; }
    public static string CertPassword { get; set; }
}
