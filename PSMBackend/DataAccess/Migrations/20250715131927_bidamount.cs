using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSMDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class bidamount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                column: "AwadedTenderId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Tenders",
                keyColumn: "Id",
                keyValue: 1,
                column: "BidAmount",
                value: 500000m);

            migrationBuilder.UpdateData(
                table: "Tenders",
                keyColumn: "Id",
                keyValue: 2,
                column: "BidAmount",
                value: 300000m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                column: "AwadedTenderId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tenders",
                keyColumn: "Id",
                keyValue: 1,
                column: "BidAmount",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Tenders",
                keyColumn: "Id",
                keyValue: 2,
                column: "BidAmount",
                value: 0m);
        }
    }
}
