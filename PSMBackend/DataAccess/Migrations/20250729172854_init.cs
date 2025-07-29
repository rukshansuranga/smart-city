using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PSMDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Mobile = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    AddressLine1 = table.Column<string>(type: "text", nullable: true),
                    AddressLine2 = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    AddressLine1 = table.Column<string>(type: "text", nullable: false),
                    AddressLine2 = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: false),
                    Mobile = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "LightPosts",
                columns: table => new
                {
                    LightPostNumber = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LightPosts", x => x.LightPostNumber);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Subject = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: true),
                    SpecificationDocument = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<LocalDate>(type: "date", nullable: true),
                    EndDate = table.Column<LocalDate>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: true),
                    Location = table.Column<string>(type: "text", nullable: true),
                    LocationNote = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    EstimatedCost = table.Column<decimal>(type: "numeric", nullable: true),
                    AwadedTenderId = table.Column<int>(type: "integer", nullable: true),
                    TenderOpeningDate = table.Column<LocalDate>(type: "date", nullable: true),
                    TenderClosingDate = table.Column<LocalDate>(type: "date", nullable: true),
                    CreatedDate = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    RegionNo = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.RegionNo);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    RouteNo = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Distance = table.Column<float>(type: "real", nullable: false),
                    CreatedDate = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.RouteNo);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Mobile = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    AddressLine1 = table.Column<string>(type: "text", nullable: true),
                    AddressLine2 = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    UserType = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    LicenseNo = table.Column<string>(type: "text", nullable: true),
                    ExpireDate = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Vehicals",
                columns: table => new
                {
                    VehicalNo = table.Column<string>(type: "text", nullable: false),
                    RegistrationNo = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    Year = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicals", x => x.VehicalNo);
                });

            migrationBuilder.CreateTable(
                name: "Tenders",
                columns: table => new
                {
                    TenderId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Subject = table.Column<string>(type: "text", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    BidAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    SubmittedDate = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenders", x => x.TenderId);
                    table.ForeignKey(
                        name: "FK_Tenders_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tenders_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Workpackages",
                columns: table => new
                {
                    WorkpackageId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Subject = table.Column<string>(type: "text", nullable: false),
                    Detail = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: true),
                    ClientId = table.Column<int>(type: "integer", nullable: true),
                    WorkpackageType = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    CreatedDate = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    GarbagePointNo = table.Column<string>(type: "text", nullable: true),
                    IsPrivate = table.Column<bool>(type: "boolean", nullable: true),
                    LightPostNumber = table.Column<string>(type: "text", nullable: true),
                    ProjectId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workpackages", x => x.WorkpackageId);
                    table.ForeignKey(
                        name: "FK_Workpackages_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId");
                    table.ForeignKey(
                        name: "FK_Workpackages_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GCShedules",
                columns: table => new
                {
                    GCSheduleId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Day = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Time = table.Column<string>(type: "text", nullable: false),
                    RegionNo = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GCShedules", x => x.GCSheduleId);
                    table.ForeignKey(
                        name: "FK_GCShedules_Regions_RegionNo",
                        column: x => x.RegionNo,
                        principalTable: "Regions",
                        principalColumn: "RegionNo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoutePoints",
                columns: table => new
                {
                    RoutePointId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PointName = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    RouteNo = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoutePoints", x => x.RoutePointId);
                    table.ForeignKey(
                        name: "FK_RoutePoints_Routes_RouteNo",
                        column: x => x.RouteNo,
                        principalTable: "Routes",
                        principalColumn: "RouteNo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    TicketId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Subject = table.Column<string>(type: "text", nullable: false),
                    Detail = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: true),
                    Estimation = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: true),
                    DueDate = table.Column<LocalDate>(type: "date", nullable: true),
                    Attachments = table.Column<List<string>>(type: "text[]", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    CreatedDate = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.TicketId);
                    table.ForeignKey(
                        name: "FK_Tickets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Rides",
                columns: table => new
                {
                    RideId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartTime = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false),
                    EndTime = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    DriverNo = table.Column<int>(type: "integer", nullable: false),
                    Driver = table.Column<int>(type: "integer", nullable: false),
                    VehicalNo = table.Column<string>(type: "text", nullable: false),
                    RegionNo = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rides", x => x.RideId);
                    table.ForeignKey(
                        name: "FK_Rides_Regions_RegionNo",
                        column: x => x.RegionNo,
                        principalTable: "Regions",
                        principalColumn: "RegionNo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rides_Vehicals_VehicalNo",
                        column: x => x.VehicalNo,
                        principalTable: "Vehicals",
                        principalColumn: "VehicalNo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "text", nullable: false),
                    WorkpackageId = table.Column<int>(type: "integer", nullable: false),
                    IsPrivate = table.Column<bool>(type: "boolean", nullable: true),
                    ClientId = table.Column<int>(type: "integer", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comments_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId");
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Comments_Workpackages_WorkpackageId",
                        column: x => x.WorkpackageId,
                        principalTable: "Workpackages",
                        principalColumn: "WorkpackageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketHistories",
                columns: table => new
                {
                    TicketHistoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TicketId = table.Column<int>(type: "integer", nullable: false),
                    PropertyName = table.Column<string>(type: "text", nullable: false),
                    OldValue = table.Column<string>(type: "text", nullable: false),
                    NewValue = table.Column<string>(type: "text", nullable: false),
                    ChangedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketHistories", x => x.TicketHistoryId);
                    table.ForeignKey(
                        name: "FK_TicketHistories_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketPackages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TicketId = table.Column<int>(type: "integer", nullable: false),
                    WorkpackageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketPackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketPackages_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketPackages_Workpackages_WorkpackageId",
                        column: x => x.WorkpackageId,
                        principalTable: "Workpackages",
                        principalColumn: "WorkpackageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RidePoints",
                columns: table => new
                {
                    RidePointId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    PointTime = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false),
                    RideId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RidePoints", x => x.RidePointId);
                    table.ForeignKey(
                        name: "FK_RidePoints_Rides_RideId",
                        column: x => x.RideId,
                        principalTable: "Rides",
                        principalColumn: "RideId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "ClientId", "AddressLine1", "AddressLine2", "City", "CreatedAt", "CreatedBy", "Email", "FirstName", "IsActive", "LastName", "Mobile", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "123 Main St", "Apt 4B", "Colombo", new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), 1, "jeewan@example.com", "Jeewan", true, "Perera", "0714789562", null, null },
                    { 2, "456 Lake Rd", null, "Kandy", new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), 1, "lakshitha@example.com", "Lakshitha", true, "Fernando", "07777894562", null, null }
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyId", "AddressLine1", "AddressLine2", "City", "CreatedAt", "CreatedBy", "IsActive", "Mobile", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "123 Main St", null, "Colombo", new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), 1, true, "0771234567", "Maga", null, null },
                    { 2, "123 Main St", null, "Gampaha", new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), 1, true, "0779876543", "Access Construction", null, null }
                });

            migrationBuilder.InsertData(
                table: "LightPosts",
                columns: new[] { "LightPostNumber", "Latitude", "Longitude" },
                values: new object[,]
                {
                    { "LP001", 6.9271000000000003, 79.861199999999997 },
                    { "LP002", 6.9272, 79.8613 }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "AwadedTenderId", "City", "CreatedAt", "CreatedBy", "Description", "EndDate", "EstimatedCost", "IsActive", "Latitude", "Location", "LocationNote", "Longitude", "SpecificationDocument", "StartDate", "Status", "Subject", "TenderClosingDate", "TenderOpeningDate", "Type", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 1, "Weliveriya", new NodaTime.LocalDateTime(2023, 1, 1, 9, 0), 1, "Description for Project Alpha", new NodaTime.LocalDate(2023, 12, 31), 1000000m, true, 34.052199999999999, "Location A", "Note for Location A", -118.2437, "specification_alpha.pdf", new NodaTime.LocalDate(2023, 1, 1), 0, "Project Alpha", new NodaTime.LocalDate(2023, 1, 30), new NodaTime.LocalDate(2023, 1, 15), 0, null, null },
                    { 2, 2, "Ambaraluwa", new NodaTime.LocalDateTime(2023, 1, 1, 9, 0), 1, "Weliveriya road project", new NodaTime.LocalDate(2023, 12, 31), 1000000m, true, 34.052199999999999, "Location A", "Note for Location A", -118.2437, "specification_alpha.pdf", new NodaTime.LocalDate(2023, 1, 1), 0, "Weliveriya road project", new NodaTime.LocalDate(2023, 1, 30), new NodaTime.LocalDate(2023, 1, 15), 0, null, null }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "RegionNo", "CreatedAt", "CreatedBy", "IsActive", "Name", "Note", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { "R001", new NodaTime.LocalDateTime(2023, 1, 1, 9, 0), 1, true, "Weliveriya South", "This is a region in Gampaha district.", null, null },
                    { "R002", new NodaTime.LocalDateTime(2023, 1, 1, 9, 0), 1, true, "Weliveriya North", "This is a region in Gampaha district.", null, null }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "RouteNo", "CreatedAt", "CreatedBy", "Distance", "IsActive", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { "R001", new NodaTime.LocalDateTime(2023, 1, 1, 9, 0), 1, 0f, true, "Noth Root", null, null },
                    { "R002", new NodaTime.LocalDateTime(2023, 1, 1, 9, 0), 1, 0f, true, "South Root", null, null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "AddressLine1", "AddressLine2", "City", "CreatedAt", "CreatedBy", "Email", "FirstName", "IsActive", "LastName", "Mobile", "UpdatedAt", "UpdatedBy", "UserType" },
                values: new object[,]
                {
                    { 1, null, null, "Colombo", new NodaTime.LocalDateTime(1, 1, 1, 0, 0), null, "admin@example.com", "Admin", true, "User", "0777582696", null, null, "User" },
                    { 2, null, null, "Galle", new NodaTime.LocalDateTime(1, 1, 1, 0, 0), null, "kamal@example.com", "Kamal", true, "Perera", "0147894562", null, null, "User" },
                    { 3, null, null, "Kandy", new NodaTime.LocalDateTime(1, 1, 1, 0, 0), null, "kumara@example.com", "Kumara", true, "Fernando", "0117654321", null, null, "User" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "AddressLine1", "AddressLine2", "City", "CreatedAt", "CreatedBy", "Email", "ExpireDate", "FirstName", "IsActive", "LastName", "LicenseNo", "Mobile", "UpdatedAt", "UpdatedBy", "UserType" },
                values: new object[,]
                {
                    { 4, null, null, "Colombo", new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), 1, null, new NodaTime.LocalDateTime(2027, 6, 19, 14, 14), "Upul", true, "Perera", "2345", "0777582365", null, null, "Driver" },
                    { 5, null, null, "Galle", new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), 1, null, new NodaTime.LocalDateTime(2028, 6, 19, 14, 14), "Shantha", true, "Fernando", "2345", "0147894492", null, null, "Driver" }
                });

            migrationBuilder.InsertData(
                table: "Vehicals",
                columns: new[] { "VehicalNo", "Brand", "CreatedAt", "CreatedBy", "IsActive", "Model", "RegistrationNo", "UpdatedAt", "UpdatedBy", "Year" },
                values: new object[,]
                {
                    { "T01", "John Dear", new NodaTime.LocalDateTime(2023, 1, 1, 9, 0), 1, true, "T66", "GA1234", null, null, "1998" },
                    { "T02", "Toyota", new NodaTime.LocalDateTime(2023, 1, 1, 9, 0), 1, true, "Truch", "CAG3456", null, null, "2024" }
                });

            migrationBuilder.InsertData(
                table: "GCShedules",
                columns: new[] { "GCSheduleId", "CreatedAt", "CreatedBy", "Day", "IsActive", "RegionNo", "Time", "Type", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new NodaTime.LocalDateTime(1, 1, 1, 0, 0), null, "Monday", true, "R001", "Morning", "Plastic", null, null },
                    { 2, new NodaTime.LocalDateTime(1, 1, 1, 0, 0), null, "Tuesday", true, "R001", "Morning", "Garbage", null, null },
                    { 3, new NodaTime.LocalDateTime(1, 1, 1, 0, 0), null, "Wednesday", true, "R001", "Morning", "Garbage", null, null },
                    { 4, new NodaTime.LocalDateTime(1, 1, 1, 0, 0), null, "Thursday", true, "R001", "Afternoon", "Garbage", null, null },
                    { 5, new NodaTime.LocalDateTime(1, 1, 1, 0, 0), null, "Friday", true, "R001", "Morning", "Paper", null, null },
                    { 6, new NodaTime.LocalDateTime(1, 1, 1, 0, 0), null, "Monday", true, "R002", "Morning", "Plastic", null, null },
                    { 7, new NodaTime.LocalDateTime(1, 1, 1, 0, 0), null, "Tuesday", true, "R002", "Morning", "Garbage", null, null },
                    { 8, new NodaTime.LocalDateTime(1, 1, 1, 0, 0), null, "Wednesday", true, "R002", "Morning", "Garbage", null, null },
                    { 9, new NodaTime.LocalDateTime(1, 1, 1, 0, 0), null, "Thursday", true, "R002", "Afternoon", "Garbage", null, null },
                    { 10, new NodaTime.LocalDateTime(1, 1, 1, 0, 0), null, "Friday", true, "R002", "Morning", "Paper", null, null }
                });

            migrationBuilder.InsertData(
                table: "Rides",
                columns: new[] { "RideId", "CreatedAt", "CreatedBy", "Driver", "DriverNo", "EndTime", "IsActive", "Notes", "RegionNo", "StartTime", "Type", "UpdatedAt", "UpdatedBy", "VehicalNo" },
                values: new object[] { 1, new NodaTime.LocalDateTime(2025, 7, 10, 12, 0), 1, 0, 1, null, true, null, "R001", new NodaTime.LocalDateTime(2025, 7, 10, 13, 0), "Paper Collection", null, null, "T01" });

            migrationBuilder.InsertData(
                table: "RoutePoints",
                columns: new[] { "RoutePointId", "CreatedAt", "CreatedBy", "IsActive", "Latitude", "Longitude", "PointName", "RouteNo", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new NodaTime.LocalDateTime(2023, 1, 1, 9, 0), 1, true, 80.026488999999998, 7.0249980000000001, "maduma bankara vidyalaya", "R001", null, null },
                    { 2, new NodaTime.LocalDateTime(2023, 1, 1, 9, 0), 1, true, 80.022924000000003, 7.0254649999999996, "gala gawa", "R001", null, null },
                    { 3, new NodaTime.LocalDateTime(2023, 1, 1, 9, 0), 1, true, 80.021614, 7.0225910000000002, "Sewa Piyasa", "R001", null, null },
                    { 4, new NodaTime.LocalDateTime(2023, 1, 1, 9, 0), 1, true, 80.023321999999993, 7.0228789999999996, "jayamal oil", "R001", null, null },
                    { 5, new NodaTime.LocalDateTime(2023, 1, 1, 9, 0), 1, true, 80.026511999999997, 7.0212519999999996, "bathiya home", "R001", null, null },
                    { 6, new NodaTime.LocalDateTime(2023, 1, 1, 9, 0), 1, true, 80.026774000000003, 7.0229679999999997, "grace health", "R001", null, null }
                });

            migrationBuilder.InsertData(
                table: "Tenders",
                columns: new[] { "TenderId", "BidAmount", "CompanyId", "CreatedAt", "CreatedBy", "IsActive", "Note", "ProjectId", "Subject", "SubmittedDate", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 500000m, 1, new NodaTime.LocalDateTime(2023, 10, 1, 9, 0), 1, true, "This is a tender for Project A", 1, "Tender for Project A", new NodaTime.LocalDateTime(2023, 10, 1, 10, 0), null, null },
                    { 2, 300000m, 1, new NodaTime.LocalDateTime(2023, 10, 1, 9, 0), 1, true, "This is a tender for Project A", 1, "Tender for Road Construction", new NodaTime.LocalDateTime(2023, 10, 1, 10, 0), null, null }
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "TicketId", "Attachments", "CreatedAt", "CreatedBy", "Detail", "DueDate", "Estimation", "IsActive", "Note", "Priority", "Status", "Subject", "Type", "UpdatedAt", "UpdatedBy", "UserId" },
                values: new object[,]
                {
                    { 1, null, new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), 1, null, null, 0, true, null, null, 0, "Fix Light Post LP001", 1, null, null, 1 },
                    { 2, null, new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), 2, null, null, 0, true, null, null, 0, "Fix Light Post LP001 2", 1, null, null, 2 },
                    { 3, null, new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), 2, null, null, 0, true, null, null, 0, "Fix Light Post LP001 3", 1, null, null, 2 },
                    { 4, null, new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), 2, null, null, 0, true, null, null, 0, "Fix Light Post LP001 4", 1, null, null, 2 }
                });

            migrationBuilder.InsertData(
                table: "Workpackages",
                columns: new[] { "WorkpackageId", "ClientId", "CreatedAt", "CreatedBy", "Detail", "IsActive", "Status", "Subject", "UpdatedAt", "UpdatedBy", "WorkpackageType" },
                values: new object[,]
                {
                    { 1, 2, new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), 1, "Detail for Work Package 1", true, 0, "Work Package 1", new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), null, "Workpackage" },
                    { 2, 2, new NodaTime.LocalDateTime(2025, 7, 1, 10, 0), 2, "Detail for Work Package 2", true, 1, "Work Package 2", new NodaTime.LocalDateTime(2025, 7, 1, 10, 0), null, "Workpackage" },
                    { 3, 2, new NodaTime.LocalDateTime(2025, 7, 10, 9, 30), 3, "Detail for Work Package 3", true, 1, "Work Package 3", new NodaTime.LocalDateTime(2025, 7, 10, 9, 30), null, "Workpackage" },
                    { 4, 2, new NodaTime.LocalDateTime(2025, 7, 15, 8, 0), 4, "Detail for Work Package 4", true, 0, "Work Package 4", new NodaTime.LocalDateTime(2025, 7, 15, 8, 0), null, "Workpackage" },
                    { 5, 2, new NodaTime.LocalDateTime(2025, 7, 20, 11, 45), 1, "Detail for Work Package 5", true, 0, "Work Package 5", new NodaTime.LocalDateTime(2025, 7, 20, 11, 45), null, "Workpackage" }
                });

            migrationBuilder.InsertData(
                table: "Workpackages",
                columns: new[] { "WorkpackageId", "ClientId", "CreatedAt", "CreatedBy", "Detail", "IsActive", "IsPrivate", "Status", "Subject", "UpdatedAt", "UpdatedBy", "WorkpackageType" },
                values: new object[] { 12, 1, new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), 1, "General Complain 1 description", true, false, 0, "General Complain 1", null, null, "GeneralComplain" });

            migrationBuilder.InsertData(
                table: "Workpackages",
                columns: new[] { "WorkpackageId", "ClientId", "CreatedAt", "CreatedBy", "Detail", "IsActive", "LightPostNumber", "Status", "Subject", "UpdatedAt", "UpdatedBy", "WorkpackageType" },
                values: new object[,]
                {
                    { 21, 1, new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), 1, "Light post not working at Main St.", true, "LP001", 0, "Light Post Issue", null, null, "LightPostComplain" },
                    { 22, 1, new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), 1, "Light post LP001 is not working 2", true, "LP001", 0, "Light Post Issue 2", null, null, "LightPostComplain" },
                    { 23, 1, new NodaTime.LocalDateTime(2025, 6, 20, 14, 14), 1, "Light post LP001 is not working 3", true, "LP001", 0, "Light Post Issue 3", null, null, "LightPostComplain" },
                    { 24, 1, new NodaTime.LocalDateTime(2025, 6, 20, 14, 14), 1, "Light post LP002 is not working 4", true, "LP002", 0, "Light Post Issue 4", null, null, "LightPostComplain" },
                    { 25, 1, new NodaTime.LocalDateTime(2025, 6, 20, 14, 14), 1, "Light post LP002 is not working 5", true, "LP002", 0, "Light Post Issue 5", null, null, "LightPostComplain" }
                });

            migrationBuilder.InsertData(
                table: "Workpackages",
                columns: new[] { "WorkpackageId", "ClientId", "CreatedAt", "CreatedBy", "Detail", "IsActive", "ProjectId", "Status", "Subject", "UpdatedAt", "UpdatedBy", "WorkpackageType" },
                values: new object[,]
                {
                    { 31, 1, new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), 1, "Project Complain 1 description", true, 1, 0, "Project Complain 1", null, null, "ProjectComplain" },
                    { 32, 2, new NodaTime.LocalDateTime(2025, 6, 20, 10, 0), 2, "Project Complain 2 description", true, 2, 1, "Project Complain 2", null, null, "ProjectComplain" }
                });

            migrationBuilder.InsertData(
                table: "TicketPackages",
                columns: new[] { "Id", "TicketId", "WorkpackageId" },
                values: new object[,]
                {
                    { 1, 1, 2 },
                    { 2, 1, 3 },
                    { 3, 3, 4 },
                    { 4, 4, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ClientId",
                table: "Comments",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_WorkpackageId",
                table: "Comments",
                column: "WorkpackageId");

            migrationBuilder.CreateIndex(
                name: "IX_GCShedules_RegionNo",
                table: "GCShedules",
                column: "RegionNo");

            migrationBuilder.CreateIndex(
                name: "IX_RidePoints_RideId",
                table: "RidePoints",
                column: "RideId");

            migrationBuilder.CreateIndex(
                name: "IX_Rides_RegionNo",
                table: "Rides",
                column: "RegionNo");

            migrationBuilder.CreateIndex(
                name: "IX_Rides_VehicalNo",
                table: "Rides",
                column: "VehicalNo");

            migrationBuilder.CreateIndex(
                name: "IX_RoutePoints_RouteNo",
                table: "RoutePoints",
                column: "RouteNo");

            migrationBuilder.CreateIndex(
                name: "IX_Tenders_CompanyId",
                table: "Tenders",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenders_ProjectId",
                table: "Tenders",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketHistories_TicketId",
                table: "TicketHistories",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketPackages_TicketId",
                table: "TicketPackages",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketPackages_WorkpackageId",
                table: "TicketPackages",
                column: "WorkpackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_UserId",
                table: "Tickets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Workpackages_ClientId",
                table: "Workpackages",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Workpackages_ProjectId",
                table: "Workpackages",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "GCShedules");

            migrationBuilder.DropTable(
                name: "LightPosts");

            migrationBuilder.DropTable(
                name: "RidePoints");

            migrationBuilder.DropTable(
                name: "RoutePoints");

            migrationBuilder.DropTable(
                name: "Tenders");

            migrationBuilder.DropTable(
                name: "TicketHistories");

            migrationBuilder.DropTable(
                name: "TicketPackages");

            migrationBuilder.DropTable(
                name: "Rides");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Workpackages");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "Vehicals");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
