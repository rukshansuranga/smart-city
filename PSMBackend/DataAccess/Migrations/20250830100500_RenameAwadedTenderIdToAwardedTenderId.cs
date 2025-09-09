using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSMDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RenameAwadedTenderIdToAwardedTenderId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AwadedTenderId",
                table: "Projects",
                newName: "AwardedTenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AwardedTenderId",
                table: "Projects",
                newName: "AwadedTenderId");
        }
    }
}
