using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PSMDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class region : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RegionNo",
                table: "Rides",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    RegionNo = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.RegionNo);
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "RegionNo", "Name", "Note" },
                values: new object[,]
                {
                    { "R001", "Weliveriya South", "This is a region in Gampaha district." },
                    { "R002", "Weliveriya North", "This is a region in Gampaha district." }
                });

            migrationBuilder.UpdateData(
                table: "Rides",
                keyColumn: "Id",
                keyValue: 1,
                column: "RegionNo",
                value: "R001");

            migrationBuilder.CreateIndex(
                name: "IX_Rides_RegionNo",
                table: "Rides",
                column: "RegionNo");

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_Regions_RegionNo",
                table: "Rides",
                column: "RegionNo",
                principalTable: "Regions",
                principalColumn: "RegionNo",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Regions_RegionNo",
                table: "Rides");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Rides_RegionNo",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "RegionNo",
                table: "Rides");
        }
    }
}
