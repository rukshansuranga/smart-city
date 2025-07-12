using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace PSMDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class rideseed1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Rides",
                columns: new[] { "Id", "Driver", "DriverNo", "EndTime", "Notes", "StartTime", "Type", "VehicalNo" },
                values: new object[] { 1, 0, 1, null, null, new NodaTime.LocalDateTime(2025, 7, 10, 13, 0), "Paper Collection", "T01" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rides",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
