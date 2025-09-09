using System.ComponentModel;
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

// Register type converter for nullable LocalDate (handles null values in multipart form data)
TypeDescriptor.AddAttributes(typeof(LocalDate?), new TypeConverterAttribute(typeof(NullableLocalDateTypeConverter)));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
});

builder.Services.AddCors();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithAuth(builder.Configuration);

// Register DbContext with dynamic connection string
builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
    var httpContext = httpContextAccessor.HttpContext;
    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (httpContext != null && httpContext.Items.ContainsKey("DynamicConnectionString"))
    {
        connectionString = httpContext.Items["DynamicConnectionString"] as string ?? connectionString;
    }
    options.UseNpgsql(connectionString, o => o.UseNodaTime());
});

// Register repositories and services
builder.Services.AddScoped<IComplainRepository, ComplainRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IGarbageRepository, GarbageRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITenderRepository, TenderRepository>();
builder.Services.AddScoped<IMiscRepository, MiscRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<ITicketActivityRepository, TicketActivityRepository>();

// Register Attachment Service
builder.Services.AddScoped<IAttachmentService, AttachmentService>();

// Register Tag Service
builder.Services.AddScoped<ITagService, TagService>();

// Configure Keycloak Client Credentials
builder.Services.Configure<KeycloakClientCredentialsOptions>(
    builder.Configuration.GetSection("keycloak:ClientCredentials"));

// Configure Keycloak Admin Token
builder.Services.Configure<KeycloakAdminTokenOptions>(
    builder.Configuration.GetSection("keycloak:AdminToken"));

// Register Keycloak Token Service
builder.Services.AddHttpClient<IKeycloakTokenService, KeycloakTokenService>();

// Register Keycloak User Service
builder.Services.AddHttpClient<IKeycloakUserService, KeycloakUserService>();

// builder.Services.AddHostedService<RideSimulationService>();
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
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
builder.Services.AddHttpContextAccessor();

var app = builder.Build();



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

// Middleware to extract council value and set connection string
app.Use(async (context, next) =>
{
    var accessToken = context.Request.Headers["Authorization"].FirstOrDefault();
    var user = context.User;
    string councilValue = user?.Claims?.FirstOrDefault(c => c.Type == "council")?.Value;
    if (!string.IsNullOrEmpty(councilValue))
    {
        var config = context.RequestServices.GetRequiredService<IConfiguration>();
        string connStrName = $"{councilValue}Connection";
        string dynamicConnStr = config.GetConnectionString(connStrName) ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(dynamicConnStr))
        {
            context.Items["DynamicConnectionString"] = dynamicConnStr;
        }
    }
    await next();
});

app.MapControllers();
app.Run();



