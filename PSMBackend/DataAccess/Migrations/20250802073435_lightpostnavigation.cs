using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSMDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class lightpostnavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Workpackages_LightPostNumber",
                table: "Workpackages",
                column: "LightPostNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Workpackages_LightPosts_LightPostNumber",
                table: "Workpackages",
                column: "LightPostNumber",
                principalTable: "LightPosts",
                principalColumn: "LightPostNumber",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workpackages_LightPosts_LightPostNumber",
                table: "Workpackages");

            migrationBuilder.DropIndex(
                name: "IX_Workpackages_LightPostNumber",
                table: "Workpackages");
        }
    }
}
