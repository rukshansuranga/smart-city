using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PSMDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addconfigtomodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Name", "TelNumber", "UserType" },
                values: new object[,]
                {
                    { 1, "Admin", "0777582696", "User" },
                    { 2, "Kamal", "0147894562", "User" },
                    { 3, "Kumara", "0117654321", "User" }
                });

            migrationBuilder.InsertData(
                table: "Vehicals",
                columns: new[] { "VehicalNo", "Brand", "Model", "RegistrationNo", "Year" },
                values: new object[,]
                {
                    { "T01", "John Dear", "T66", "GA1234", "1998" },
                    { "T02", "Toyota", "Truch", "CAG3456", "2024" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Vehicals",
                keyColumn: "VehicalNo",
                keyValue: "T01");

            migrationBuilder.DeleteData(
                table: "Vehicals",
                keyColumn: "VehicalNo",
                keyValue: "T02");
        }
    }
}
