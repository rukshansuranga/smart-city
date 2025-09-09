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
                    ClientId = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Mobile = table.Column<string>(type: "text", nullable: true),
                    AddressLine1 = table.Column<string>(type: "text", nullable: true),
                    AddressLine2 = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "Contractors",
                columns: table => new
                {
                    ContractorId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    AddressLine1 = table.Column<string>(type: "text", nullable: false),
                    AddressLine2 = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: false),
                    Mobile = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contractors", x => x.ContractorId);
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
                    ProgressFrequency = table.Column<int>(type: "integer", nullable: true),
                    TenderOpeningDate = table.Column<LocalDate>(type: "date", nullable: true),
                    TenderClosingDate = table.Column<LocalDate>(type: "date", nullable: true),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
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
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
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
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
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
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Mobile = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    AddressLine1 = table.Column<string>(type: "text", nullable: true),
                    AddressLine2 = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: false),
                    Designation = table.Column<int>(type: "integer", nullable: true),
                    UserType = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    LicenseNo = table.Column<string>(type: "text", nullable: true),
                    ExpireDate = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
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
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicals", x => x.VehicalNo);
                });

            migrationBuilder.CreateTable(
                name: "Complains",
                columns: table => new
                {
                    ComplainId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Subject = table.Column<string>(type: "text", nullable: false),
                    Detail = table.Column<string>(type: "text", nullable: true),
                    Rating = table.Column<double>(type: "double precision", nullable: true),
                    RatedBy = table.Column<string>(type: "text", nullable: true),
                    RatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    RatingReview = table.Column<string>(type: "text", nullable: true),
                    Sentiment = table.Column<string>(type: "text", nullable: true),
                    Summary = table.Column<string>(type: "text", nullable: true),
                    MyProperty = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: true),
                    ClientId = table.Column<string>(type: "text", nullable: true),
                    ComplainType = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    GarbagePointNo = table.Column<string>(type: "text", nullable: true),
                    IsPrivate = table.Column<bool>(type: "boolean", nullable: true),
                    LightPostNumber = table.Column<string>(type: "text", nullable: true),
                    ProjectId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complains", x => x.ComplainId);
                    table.ForeignKey(
                        name: "FK_Complains_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId");
                    table.ForeignKey(
                        name: "FK_Complains_LightPosts_LightPostNumber",
                        column: x => x.LightPostNumber,
                        principalTable: "LightPosts",
                        principalColumn: "LightPostNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Complains_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    TenderDocument = table.Column<string>(type: "text", nullable: true),
                    SubmittedDate = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    ContractorId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenders", x => x.TenderId);
                    table.ForeignKey(
                        name: "FK_Tenders_Contractors_ContractorId",
                        column: x => x.ContractorId,
                        principalTable: "Contractors",
                        principalColumn: "ContractorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tenders_Projects_ProjectId",
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
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
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
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
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
                name: "ProjectInspectors",
                columns: table => new
                {
                    ProjectInspectorId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    InspectorId = table.Column<string>(type: "text", nullable: false),
                    AssignDate = table.Column<LocalDate>(type: "date", nullable: false),
                    Responsibility = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectInspectors", x => x.ProjectInspectorId);
                    table.ForeignKey(
                        name: "FK_ProjectInspectors_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectInspectors_Users_InspectorId",
                        column: x => x.InspectorId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectProgresses",
                columns: table => new
                {
                    ProjectProgressId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ProgressDate = table.Column<LocalDate>(type: "date", nullable: false),
                    ProgressPercentage = table.Column<double>(type: "double precision", nullable: false),
                    ApprovedBy = table.Column<string>(type: "text", nullable: true),
                    ApprovedAt = table.Column<LocalDate>(type: "date", nullable: true),
                    ApprovedNote = table.Column<string>(type: "text", nullable: true),
                    ProjectProgressApprovedStatus = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectProgresses", x => x.ProjectProgressId);
                    table.ForeignKey(
                        name: "FK_ProjectProgresses_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectProgresses_Users_ApprovedBy",
                        column: x => x.ApprovedBy,
                        principalTable: "Users",
                        principalColumn: "UserId");
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
                    Tags = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: true),
                    Estimation = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: true),
                    DueDate = table.Column<LocalDate>(type: "date", nullable: true),
                    Attachments = table.Column<List<string>>(type: "text[]", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    TicketType = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    ComplainType = table.Column<int>(type: "integer", nullable: true),
                    ProjectId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.TicketId);
                    table.ForeignKey(
                        name: "FK_Tickets_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
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
                    ComplainId = table.Column<int>(type: "integer", nullable: false),
                    IsPrivate = table.Column<bool>(type: "boolean", nullable: true),
                    ClientId = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
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
                        name: "FK_Comments_Complains_ComplainId",
                        column: x => x.ComplainId,
                        principalTable: "Complains",
                        principalColumn: "ComplainId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "ProjectInspections",
                columns: table => new
                {
                    ProjectInspectionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectInspectorId = table.Column<int>(type: "integer", nullable: false),
                    InspectionDate = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false),
                    Findings = table.Column<string>(type: "text", nullable: true),
                    Recommendations = table.Column<string>(type: "text", nullable: true),
                    TicketId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectInspections", x => x.ProjectInspectionId);
                    table.ForeignKey(
                        name: "FK_ProjectInspections_ProjectInspectors_ProjectInspectorId",
                        column: x => x.ProjectInspectorId,
                        principalTable: "ProjectInspectors",
                        principalColumn: "ProjectInspectorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Subject = table.Column<string>(type: "text", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true),
                    ClientId = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    TicketId = table.Column<int>(type: "integer", nullable: true),
                    ComplainId = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    Error = table.Column<string>(type: "text", nullable: true),
                    RetryCount = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Complains_ComplainId",
                        column: x => x.ComplainId,
                        principalTable: "Complains",
                        principalColumn: "ComplainId");
                    table.ForeignKey(
                        name: "FK_Notifications_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "TicketId");
                });

            migrationBuilder.CreateTable(
                name: "TicketActivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TicketId = table.Column<int>(type: "integer", nullable: false),
                    Transition = table.Column<int>(type: "integer", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketActivities_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketActivities_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
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
                    OldValue = table.Column<string>(type: "text", nullable: true),
                    NewValue = table.Column<string>(type: "text", nullable: true),
                    ChangedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<string>(type: "text", nullable: false)
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
                    ComplainId = table.Column<int>(type: "integer", nullable: false),
                    ComplainTicketTicketId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketPackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketPackages_Complains_ComplainId",
                        column: x => x.ComplainId,
                        principalTable: "Complains",
                        principalColumn: "ComplainId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketPackages_Tickets_ComplainTicketTicketId",
                        column: x => x.ComplainTicketTicketId,
                        principalTable: "Tickets",
                        principalColumn: "TicketId");
                    table.ForeignKey(
                        name: "FK_TicketPackages_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "TicketId",
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
                columns: new[] { "ClientId", "AddressLine1", "AddressLine2", "City", "CreatedAt", "CreatedBy", "FirstName", "IsActive", "LastName", "Mobile", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { "1", "123 Main St", "Apt 4B", "Colombo", new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), "1", null, true, null, null, null, null },
                    { "2", "456 Lake Rd", null, "Kandy", new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), "1", null, true, null, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "Contractors",
                columns: new[] { "ContractorId", "AddressLine1", "AddressLine2", "City", "CreatedAt", "CreatedBy", "IsActive", "Mobile", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { "a4aa5b28-36ab-4991-975a-5e5e441bf6fa", "123 Main St", null, "Gampaha", new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), "1", true, "0779876543", "Access Construction", null, null },
                    { "d0eca5fa-8cf4-4256-ab6a-9405c789c1b1", "123 Main St", null, "Colombo", new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), "1", true, "0771234567", "Maga", null, null }
                });

            migrationBuilder.InsertData(
                table: "LightPosts",
                columns: new[] { "LightPostNumber", "Latitude", "Longitude" },
                values: new object[,]
                {
                    { "LP001", 7.0259340000000003, 80.026858000000004 },
                    { "LP002", 7.027679, 80.027620999999996 },
                    { "LP003", 7.0308330000000003, 80.028531999999998 },
                    { "LP004", 7.0330199999999996, 80.027282 },
                    { "LP005", 7.0348280000000001, 80.025925999999998 }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "AwadedTenderId", "City", "CreatedAt", "CreatedBy", "Description", "EndDate", "EstimatedCost", "IsActive", "Latitude", "Location", "LocationNote", "Longitude", "ProgressFrequency", "SpecificationDocument", "StartDate", "Status", "Subject", "TenderClosingDate", "TenderOpeningDate", "Type", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 1, "Weliveriya", new NodaTime.LocalDateTime(2023, 1, 1, 9, 0), "1", "Description for Project Alpha", new NodaTime.LocalDate(2023, 12, 31), 1000000m, true, 34.052199999999999, "Location A", "Note for Location A", -118.2437, null, "specification_alpha.pdf", new NodaTime.LocalDate(2023, 1, 1), 0, "Project Alpha", new NodaTime.LocalDate(2023, 1, 30), new NodaTime.LocalDate(2023, 1, 15), 0, null, null },
                    { 2, 2, "Ambaraluwa", new NodaTime.LocalDateTime(2023, 1, 1, 9, 0), "1", "Weliveriya road project", new NodaTime.LocalDate(2023, 12, 31), 1000000m, true, 34.052199999999999, "Location A", "Note for Location A", -118.2437, null, "specification_alpha.pdf", new NodaTime.LocalDate(2023, 1, 1), 0, "Weliveriya road project", new NodaTime.LocalDate(2023, 1, 30), new NodaTime.LocalDate(2023, 1, 15), 0, null, null }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "RegionNo", "CreatedAt", "CreatedBy", "IsActive", "Name", "Note", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { "R001", new NodaTime.LocalDateTime(2023, 1, 1, 9, 0), "1", true, "Weliveriya South", "This is a region in Gampaha district.", null, null },
                    { "R002", new NodaTime.LocalDateTime(2023, 1, 1, 9, 0), "1", true, "Weliveriya North", "This is a region in Gampaha district.", null, null }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "RouteNo", "CreatedAt", "CreatedBy", "Distance", "IsActive", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { "R001", new NodaTime.LocalDateTime(2023, 1, 1, 9, 0), "1", 0f, true, "Noth Root", null, null },
                    { "R002", new NodaTime.LocalDateTime(2023, 1, 1, 9, 0), "1", 0f, true, "South Root", null, null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "AddressLine1", "AddressLine2", "City", "CreatedBy", "Designation", "Email", "FirstName", "IsActive", "LastName", "Mobile", "UpdatedAt", "UpdatedBy", "UserType" },
                values: new object[] { "0c895075-b8e6-48f9-bb9e-2c9db9d7207a", null, null, "Colombo", null, null, "ajith@example.com", "Ajith", true, "User", "0777582696", null, null, "User" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "AddressLine1", "AddressLine2", "City", "CreatedAt", "CreatedBy", "Designation", "Email", "ExpireDate", "FirstName", "IsActive", "LastName", "LicenseNo", "Mobile", "UpdatedAt", "UpdatedBy", "UserType" },
                values: new object[] { "4", null, null, "Colombo", new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), "1", null, null, new NodaTime.LocalDateTime(2027, 6, 19, 14, 14), "Upul", true, "Perera", "2345", "0777582365", null, null, "Driver" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "AddressLine1", "AddressLine2", "City", "CreatedBy", "Designation", "Email", "FirstName", "IsActive", "LastName", "Mobile", "UpdatedAt", "UpdatedBy", "UserType" },
                values: new object[] { "43e63068-a5fd-4a45-acfb-0383ff4d45ea", null, null, "Galle", null, null, "kumara@example.com", "Kumara", true, "Perera", "0147894562", null, null, "User" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "AddressLine1", "AddressLine2", "City", "CreatedAt", "CreatedBy", "Designation", "Email", "ExpireDate", "FirstName", "IsActive", "LastName", "LicenseNo", "Mobile", "UpdatedAt", "UpdatedBy", "UserType" },
                values: new object[] { "5", null, null, "Galle", new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), "1", null, null, new NodaTime.LocalDateTime(2028, 6, 19, 14, 14), "Shantha", true, "Fernando", "2345", "0147894492", null, null, "Driver" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "AddressLine1", "AddressLine2", "City", "CreatedBy", "Designation", "Email", "FirstName", "IsActive", "LastName", "Mobile", "UpdatedAt", "UpdatedBy", "UserType" },
                values: new object[,]
                {
                    { "6c35c5ad-2f70-4c3f-aa44-c94bc61d10a1", null, null, "Kandy", null, null, "upul@example.com", "Upul", true, "Fernando", "0117654321", null, null, "User" },
                    { "a4aa5b28-36ab-4991-975a-5e5e441bf6fa", null, null, "Kandy", null, null, "kamal@example.com", "constractor2", true, "Fernando", "0117654321", null, null, "User" },
                    { "d0eca5fa-8cf4-4256-ab6a-9405c789c1b1", null, null, "Kandy", null, null, "kamal@example.com", "kamal", true, "Fernando", "0117654321", null, null, "User" }
                });

            migrationBuilder.InsertData(
                table: "Vehicals",
                columns: new[] { "VehicalNo", "Brand", "CreatedAt", "CreatedBy", "IsActive", "Model", "RegistrationNo", "UpdatedAt", "UpdatedBy", "Year" },
                values: new object[,]
                {
                    { "T01", "John Dear", new NodaTime.LocalDateTime(2023, 1, 1, 9, 0), "1", true, "T66", "GA1234", null, null, "1998" },
                    { "T02", "Toyota", new NodaTime.LocalDateTime(2023, 1, 1, 9, 0), "1", true, "Truch", "CAG3456", null, null, "2024" }
                });

            migrationBuilder.InsertData(
                table: "Complains",
                columns: new[] { "ComplainId", "ClientId", "ComplainType", "CreatedAt", "CreatedBy", "Detail", "IsActive", "IsPrivate", "MyProperty", "RatedAt", "RatedBy", "Rating", "RatingReview", "Sentiment", "Status", "Subject", "Summary", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 1, "1", "GeneralComplain", new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), "1", "General Complain 1 description", true, false, 0, null, null, null, null, null, 0, "General Complain 1", null, null, null });

            migrationBuilder.InsertData(
                table: "Complains",
                columns: new[] { "ComplainId", "ClientId", "ComplainType", "CreatedAt", "CreatedBy", "Detail", "IsActive", "LightPostNumber", "MyProperty", "RatedAt", "RatedBy", "Rating", "RatingReview", "Sentiment", "Status", "Subject", "Summary", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 2, "1", "LightPostComplain", new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), "1", "Light post not working at Main St.", true, "LP001", 0, null, null, null, null, null, 0, "Light Post Issue", null, null, null },
                    { 3, "1", "LightPostComplain", new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), "1", "Light post LP001 is not working 2", true, "LP001", 0, null, null, null, null, null, 0, "Light Post Issue 2", null, null, null }
                });

            migrationBuilder.InsertData(
                table: "Complains",
                columns: new[] { "ComplainId", "ClientId", "ComplainType", "CreatedAt", "CreatedBy", "Detail", "IsActive", "MyProperty", "ProjectId", "RatedAt", "RatedBy", "Rating", "RatingReview", "Sentiment", "Status", "Subject", "Summary", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 4, "1", "ProjectComplain", new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), "1", "Project Complain 1 description", true, 0, 1, null, null, null, null, null, 0, "Project Complain 1", null, null, null },
                    { 5, "2", "ProjectComplain", new NodaTime.LocalDateTime(2025, 6, 20, 10, 0), "2", "Project Complain 2 description", true, 0, 2, null, null, null, null, null, 2, "Project Complain 2", null, null, null }
                });

            migrationBuilder.InsertData(
                table: "GCShedules",
                columns: new[] { "GCSheduleId", "CreatedBy", "Day", "IsActive", "RegionNo", "Time", "Type", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, null, "Monday", true, "R001", "Morning", "Plastic", null, null },
                    { 2, null, "Tuesday", true, "R001", "Morning", "Garbage", null, null },
                    { 3, null, "Wednesday", true, "R001", "Morning", "Garbage", null, null },
                    { 4, null, "Thursday", true, "R001", "Afternoon", "Garbage", null, null },
                    { 5, null, "Friday", true, "R001", "Morning", "Paper", null, null },
                    { 6, null, "Monday", true, "R002", "Morning", "Plastic", null, null },
                    { 7, null, "Tuesday", true, "R002", "Morning", "Garbage", null, null },
                    { 8, null, "Wednesday", true, "R002", "Morning", "Garbage", null, null },
                    { 9, null, "Thursday", true, "R002", "Afternoon", "Garbage", null, null },
                    { 10, null, "Friday", true, "R002", "Morning", "Paper", null, null }
                });

            migrationBuilder.InsertData(
                table: "ProjectInspectors",
                columns: new[] { "ProjectInspectorId", "AssignDate", "CreatedBy", "InspectorId", "IsActive", "Note", "ProjectId", "Responsibility", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new NodaTime.LocalDate(2025, 8, 24), null, "0c895075-b8e6-48f9-bb9e-2c9db9d7207a", true, "Can you provide an update on the project status?", 1, "Inspect the project", null, null },
                    { 2, new NodaTime.LocalDate(2025, 8, 24), null, "43e63068-a5fd-4a45-acfb-0383ff4d45ea", true, "Can you give support on the project?", 1, "Support", null, null },
                    { 3, new NodaTime.LocalDate(2025, 8, 24), null, "6c35c5ad-2f70-4c3f-aa44-c94bc61d10a1", true, "Can you give support on the project?", 2, "Manage", null, null }
                });

            migrationBuilder.InsertData(
                table: "Rides",
                columns: new[] { "RideId", "CreatedAt", "CreatedBy", "Driver", "DriverNo", "EndTime", "IsActive", "Notes", "RegionNo", "StartTime", "Type", "UpdatedAt", "UpdatedBy", "VehicalNo" },
                values: new object[] { 1, new NodaTime.LocalDateTime(2025, 7, 10, 12, 0), "1", 0, 1, null, true, null, "R001", new NodaTime.LocalDateTime(2025, 7, 10, 13, 0), "Paper Collection", null, null, "T01" });

            migrationBuilder.InsertData(
                table: "RoutePoints",
                columns: new[] { "RoutePointId", "CreatedAt", "CreatedBy", "IsActive", "Latitude", "Longitude", "PointName", "RouteNo", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new NodaTime.LocalDateTime(2023, 1, 1, 9, 0), "1", true, 80.026488999999998, 7.0249980000000001, "maduma bankara vidyalaya", "R001", null, null },
                    { 2, new NodaTime.LocalDateTime(2023, 1, 1, 9, 0), "1", true, 80.022924000000003, 7.0254649999999996, "gala gawa", "R001", null, null },
                    { 3, new NodaTime.LocalDateTime(2023, 1, 1, 9, 0), "1", true, 80.021614, 7.0225910000000002, "Sewa Piyasa", "R001", null, null },
                    { 4, new NodaTime.LocalDateTime(2023, 1, 1, 9, 0), "1", true, 80.023321999999993, 7.0228789999999996, "jayamal oil", "R001", null, null },
                    { 5, new NodaTime.LocalDateTime(2023, 1, 1, 9, 0), "1", true, 80.026511999999997, 7.0212519999999996, "bathiya home", "R001", null, null },
                    { 6, new NodaTime.LocalDateTime(2023, 1, 1, 9, 0), "1", true, 80.026774000000003, 7.0229679999999997, "grace health", "R001", null, null }
                });

            migrationBuilder.InsertData(
                table: "Tenders",
                columns: new[] { "TenderId", "BidAmount", "ContractorId", "CreatedAt", "CreatedBy", "IsActive", "Note", "ProjectId", "Subject", "SubmittedDate", "TenderDocument", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 500000m, "d0eca5fa-8cf4-4256-ab6a-9405c789c1b1", new NodaTime.LocalDateTime(2023, 10, 1, 9, 0), "43e63068-a5fd-4a45-acfb-0383ff4d45ea", true, "This is a tender for Project A", 1, "Tender for Project A", new NodaTime.LocalDateTime(2023, 10, 1, 10, 0), null, null, null },
                    { 2, 300000m, "d0eca5fa-8cf4-4256-ab6a-9405c789c1b1", new NodaTime.LocalDateTime(2023, 10, 1, 9, 0), "43e63068-a5fd-4a45-acfb-0383ff4d45ea", true, "This is a tender for Project A", 1, "Tender for Road Construction", new NodaTime.LocalDateTime(2023, 10, 1, 10, 0), null, null, null },
                    { 3, 300000m, "a4aa5b28-36ab-4991-975a-5e5e441bf6fa", new NodaTime.LocalDateTime(2023, 10, 1, 9, 0), "0c895075-b8e6-48f9-bb9e-2c9db9d7207a", true, "This is a tender for Project 2", 2, "Tender for Project Building", new NodaTime.LocalDateTime(2023, 10, 1, 10, 0), null, null, null },
                    { 4, 400000m, "a4aa5b28-36ab-4991-975a-5e5e441bf6fa", new NodaTime.LocalDateTime(2023, 10, 1, 9, 0), "0c895075-b8e6-48f9-bb9e-2c9db9d7207a", true, "This is a tender for Project 2", 2, "Tender for Road Building", new NodaTime.LocalDateTime(2023, 10, 1, 10, 0), null, null, null }
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "TicketId", "Attachments", "CreatedAt", "CreatedBy", "Detail", "DueDate", "Estimation", "IsActive", "Note", "Priority", "Status", "Subject", "Tags", "TicketType", "Type", "UpdatedAt", "UpdatedBy", "UserId" },
                values: new object[,]
                {
                    { 1, null, new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), "0c895075-b8e6-48f9-bb9e-2c9db9d7207a", null, null, 0, true, null, null, 0, "Fix Light Post LP001", null, "Ticket", 1, null, null, "0c895075-b8e6-48f9-bb9e-2c9db9d7207a" },
                    { 2, null, new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), "0c895075-b8e6-48f9-bb9e-2c9db9d7207a", null, null, 0, true, null, null, 0, "Fix Light Post LP001 2", null, "Ticket", 1, null, null, "0c895075-b8e6-48f9-bb9e-2c9db9d7207a" },
                    { 3, null, new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), "0c895075-b8e6-48f9-bb9e-2c9db9d7207a", null, null, 0, true, null, null, 0, "Fix Light Post LP001 3", null, "Ticket", 1, null, null, "0c895075-b8e6-48f9-bb9e-2c9db9d7207a" },
                    { 4, null, new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), "43e63068-a5fd-4a45-acfb-0383ff4d45ea", null, null, 0, true, null, null, 0, "Fix Light Post LP001 4", null, "Ticket", 1, null, null, "43e63068-a5fd-4a45-acfb-0383ff4d45ea" }
                });

            migrationBuilder.InsertData(
                table: "TicketPackages",
                columns: new[] { "Id", "ComplainId", "ComplainTicketTicketId", "TicketId" },
                values: new object[,]
                {
                    { 1, 2, null, 1 },
                    { 2, 3, null, 1 },
                    { 3, 4, null, 3 },
                    { 4, 5, null, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ClientId",
                table: "Comments",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ComplainId",
                table: "Comments",
                column: "ComplainId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Complains_ClientId",
                table: "Complains",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Complains_LightPostNumber",
                table: "Complains",
                column: "LightPostNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Complains_ProjectId",
                table: "Complains",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_GCShedules_RegionNo",
                table: "GCShedules",
                column: "RegionNo");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ComplainId",
                table: "Notifications",
                column: "ComplainId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_TicketId",
                table: "Notifications",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectInspections_ProjectInspectorId",
                table: "ProjectInspections",
                column: "ProjectInspectorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectInspectors_InspectorId",
                table: "ProjectInspectors",
                column: "InspectorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectInspectors_ProjectId",
                table: "ProjectInspectors",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectProgresses_ApprovedBy",
                table: "ProjectProgresses",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectProgresses_ProjectId",
                table: "ProjectProgresses",
                column: "ProjectId");

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
                name: "IX_Tenders_ContractorId",
                table: "Tenders",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenders_ProjectId",
                table: "Tenders",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketActivities_TicketId",
                table: "TicketActivities",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketActivities_UserId",
                table: "TicketActivities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketHistories_TicketId",
                table: "TicketHistories",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketPackages_ComplainId",
                table: "TicketPackages",
                column: "ComplainId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketPackages_ComplainTicketTicketId",
                table: "TicketPackages",
                column: "ComplainTicketTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketPackages_TicketId",
                table: "TicketPackages",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ProjectId",
                table: "Tickets",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_UserId",
                table: "Tickets",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "GCShedules");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "ProjectInspections");

            migrationBuilder.DropTable(
                name: "ProjectProgresses");

            migrationBuilder.DropTable(
                name: "RidePoints");

            migrationBuilder.DropTable(
                name: "RoutePoints");

            migrationBuilder.DropTable(
                name: "Tenders");

            migrationBuilder.DropTable(
                name: "TicketActivities");

            migrationBuilder.DropTable(
                name: "TicketHistories");

            migrationBuilder.DropTable(
                name: "TicketPackages");

            migrationBuilder.DropTable(
                name: "ProjectInspectors");

            migrationBuilder.DropTable(
                name: "Rides");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "Contractors");

            migrationBuilder.DropTable(
                name: "Complains");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "Vehicals");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "LightPosts");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
