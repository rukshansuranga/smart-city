using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace PSMDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class generalcomplainconfig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WorkPackages",
                columns: new[] { "WorkPackageId", "ClientId", "CreatedDate", "Detail", "IsPrivate", "Name", "Status", "UpdatedDate", "WorkPackageType" },
                values: new object[] { 12, 1, new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), "General Complain 1 description", false, "General Complain 1", "New", new NodaTime.LocalDateTime(2025, 6, 19, 14, 14), "GeneralComplain" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WorkPackages",
                keyColumn: "WorkPackageId",
                keyValue: 12);
        }
    }
}
