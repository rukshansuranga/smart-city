using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PSMDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class projectcomplain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "WorkPackages",
                type: "integer",
                nullable: true);

            migrationBuilder.InsertData(
                table: "WorkPackages",
                columns: new[] { "WorkPackageId", "ClientId", "CreatedDate", "Detail", "Name", "ProjectId", "Status", "UpdatedDate", "WorkPackageType" },
                values: new object[,]
                {
                    { 31, 1, new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), "Project Complain 1 description", "Project Complain 1", 1, "New", new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), "ProjectComplain" },
                    { 32, 2, new NodaTime.LocalDateTime(2025, 6, 20, 10, 0), "Project Complain 2 description", "Project Complain 2", 2, "In Progress", new NodaTime.LocalDateTime(2025, 6, 20, 10, 0), "ProjectComplain" },
                    { 33, 2, new NodaTime.LocalDateTime(2025, 6, 21, 9, 30), "Project Complain 3 description", "Project Complain 3", 3, "Resolved", new NodaTime.LocalDateTime(2025, 6, 21, 9, 30), "ProjectComplain" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WorkPackages",
                keyColumn: "WorkPackageId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "WorkPackages",
                keyColumn: "WorkPackageId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "WorkPackages",
                keyColumn: "WorkPackageId",
                keyValue: 33);

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "WorkPackages");
        }
    }
}
