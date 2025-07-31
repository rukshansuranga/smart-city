using System.Diagnostics;
using System.Net;
using System.Security.Claims;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using PSMDataAccess;
using PSMWebAPI.Extensions;
using PSMWebAPI.Profiles;
using PSMWebAPI.Repositories;
using PSMWebAPI.Services;
using PSMWebAPI.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
});

//builder.Services.AddControllers();

builder.Services.AddCors();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithAuth(builder.Configuration);

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
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITenderRepository, TenderRepository>();
builder.Services.AddScoped<IMiscRepository, MiscRepository>();

// builder.Services.AddHostedService<RideSimulationService>();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // Set to true in production
        options.MetadataAddress = builder.Configuration["Authentication:MetadataAddress"];
        options.Audience = builder.Configuration["Authentication:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            //ValidateIssuerSigningKey = true,
            //ValidateIssuer = true,
            //ValidateAudience = true,
            ValidIssuer = builder.Configuration["Authentication:ValidIssuer"],
            //ValidAudience = builder.Configuration["Authentication:Audience"]
        };
    });


builder.Services.AddAutoMapper(typeof(ProjectProfile));

var app = builder.Build();

//AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

//app.MapDefaultEndpoints();

app.MapGet("users/me", (ClaimsPrincipal claimsPrincipal) =>
{
    return claimsPrincipal.Claims.ToDictionary(c => c.Type, c => c.Value);
}).RequireAuthorization();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply migrations automatically
//ApplyMigrations(app);

app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

//app.UseHttpsRedirection();
//app.UseRouting();

// turn on PII logging
Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;


app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();

// static void ApplyMigrations(WebApplication app)
//         {
//             using (var scope = app.Services.CreateScope())
//             {
//                 var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

//                 // Check and apply pending migrations
//                 var pendingMigrations = dbContext.Database.GetPendingMigrations();
//                 if (pendingMigrations.Any())
//                 {
//                     Console.WriteLine("Applying pending migrations...");
//                     dbContext.Database.Migrate();
//                     Console.WriteLine("Migrations applied successfully.");
//                 }
//                 else
//                 {
//                     Console.WriteLine("No pending migrations found.");
//                 }
//             }
//         }


