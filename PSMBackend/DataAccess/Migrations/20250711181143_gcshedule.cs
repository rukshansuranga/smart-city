using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PSMDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class gcshedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GCShedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Day = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Time = table.Column<string>(type: "text", nullable: false),
                    RegionNo = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GCShedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GCShedules_Regions_RegionNo",
                        column: x => x.RegionNo,
                        principalTable: "Regions",
                        principalColumn: "RegionNo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "GCShedules",
                columns: new[] { "Id", "Day", "RegionNo", "Time", "Type" },
                values: new object[,]
                {
                    { 1, "Monday", "R001", "Morning", "Platic" },
                    { 2, "Tuesday", "R001", "Morning", "Garbage" },
                    { 3, "Wednesday", "R001", "Morning", "Garbage" },
                    { 4, "Thursday", "R001", "Afternoon", "Garbage" },
                    { 5, "Friday", "R001", "Morning", "Paper" },
                    { 6, "Monday", "R001", "Morning", "Platic" },
                    { 7, "Tuesday", "R002", "Morning", "Garbage" },
                    { 8, "Wednesday", "R002", "Morning", "Garbage" },
                    { 9, "Thursday", "R002", "Afternoon", "Garbage" },
                    { 10, "Friday", "R002", "Morning", "Paper" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GCShedules_RegionNo",
                table: "GCShedules",
                column: "RegionNo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GCShedules");
        }
    }
}
