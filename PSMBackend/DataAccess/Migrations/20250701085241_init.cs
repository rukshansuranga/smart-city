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
                    Name = table.Column<string>(type: "text", nullable: false),
                    Mobile = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ClientId);
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
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TelNumber = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "WorkPackages",
                columns: table => new
                {
                    WorkPackageId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Detail = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: true),
                    ClientId = table.Column<int>(type: "integer", nullable: true),
                    WorkPackageType = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    GarbagePointNo = table.Column<string>(type: "text", nullable: true),
                    LightPostNumber = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkPackages", x => x.WorkPackageId);
                    table.ForeignKey(
                        name: "FK_WorkPackages_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId");
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    TicketId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Detail = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.TicketId);
                    table.ForeignKey(
                        name: "FK_Tickets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketPackages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TicketId = table.Column<int>(type: "integer", nullable: false),
                    WorkPackageId = table.Column<int>(type: "integer", nullable: false)
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
                        name: "FK_TicketPackages_WorkPackages_WorkPackageId",
                        column: x => x.WorkPackageId,
                        principalTable: "WorkPackages",
                        principalColumn: "WorkPackageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "ClientId", "Mobile", "Name" },
                values: new object[,]
                {
                    { 1, "0714789562", "Jeewan" },
                    { 2, "07777894562", "Lakshitha" }
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
                table: "Users",
                columns: new[] { "UserId", "Name", "TelNumber" },
                values: new object[,]
                {
                    { 1, "Admin", "0777582696" },
                    { 2, "Kamal", "0147894562" },
                    { 3, "Kumara", "0117654321" }
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "TicketId", "CreatedDate", "Detail", "Note", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), null, null, "Fix Light Post LP001", 1 },
                    { 2, new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), null, null, "Fix Light Post LP001 2", 2 },
                    { 3, new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), null, null, "Fix Light Post LP001 3", 2 },
                    { 4, new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), null, null, "Fix Light Post LP001 4", 2 }
                });

            migrationBuilder.InsertData(
                table: "WorkPackages",
                columns: new[] { "WorkPackageId", "ClientId", "CreatedDate", "Detail", "Name", "Status", "UpdatedDate", "WorkPackageType" },
                values: new object[] { 1, 2, new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), "Garbage collection needed in area LP002", "Garbage Collection", "Open", new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), "WorkPackage" });

            migrationBuilder.InsertData(
                table: "WorkPackages",
                columns: new[] { "WorkPackageId", "ClientId", "CreatedDate", "Detail", "LightPostNumber", "Name", "Status", "UpdatedDate", "WorkPackageType" },
                values: new object[,]
                {
                    { 2, 1, new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), "Light post LP001 is not working", "LP001", "lightisnotworking", "Open", new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), "LightPostComplint" },
                    { 3, 1, new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), "Light post LP001 is not working 2", "LP001", "lightisnotworking", "Open", new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), "LightPostComplint" },
                    { 4, 1, new NodaTime.LocalDateTime(2025, 6, 20, 14, 14), "Light post LP001 is not working 3", "LP001", "lightisnotworking", "Open", new NodaTime.LocalDateTime(2025, 6, 20, 14, 14), "LightPostComplint" },
                    { 5, 1, new NodaTime.LocalDateTime(2025, 6, 20, 14, 14), "Light post LP001 is not working 4", "LP002", "lightisnotworking", "Open", new NodaTime.LocalDateTime(2025, 6, 20, 14, 14), "LightPostComplint" },
                    { 6, 1, new NodaTime.LocalDateTime(2025, 6, 20, 14, 14), "Light post LP001 is not working 5", "LP002", "lightisnotworking", "Open", new NodaTime.LocalDateTime(2025, 6, 20, 14, 14), "LightPostComplint" }
                });

            migrationBuilder.InsertData(
                table: "WorkPackages",
                columns: new[] { "WorkPackageId", "ClientId", "CreatedDate", "Detail", "GarbagePointNo", "Name", "Status", "UpdatedDate", "WorkPackageType" },
                values: new object[] { 7, 2, new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), "Garbage collection needed in area", "GP001", "Garbage Collection", "Open", new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), "GarbageComplain" });

            migrationBuilder.InsertData(
                table: "TicketPackages",
                columns: new[] { "Id", "TicketId", "WorkPackageId" },
                values: new object[,]
                {
                    { 1, 1, 2 },
                    { 2, 1, 3 },
                    { 3, 3, 4 },
                    { 4, 4, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketPackages_TicketId",
                table: "TicketPackages",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketPackages_WorkPackageId",
                table: "TicketPackages",
                column: "WorkPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_UserId",
                table: "Tickets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkPackages_ClientId",
                table: "WorkPackages",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LightPosts");

            migrationBuilder.DropTable(
                name: "TicketPackages");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "WorkPackages");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
