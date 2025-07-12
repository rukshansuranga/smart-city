using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PSMDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class driverfeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "ExpireDate", "LicenNo", "Name", "TelNumber", "UserType" },
                values: new object[,]
                {
                    { 4, new NodaTime.LocalDateTime(2027, 6, 19, 14, 14), "2345", "Upul", "0777582365", "Driver" },
                    { 5, new NodaTime.LocalDateTime(2028, 6, 19, 14, 14), "2345", "Shantha", "0147894492", "Driver" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 5);
        }
    }
}
