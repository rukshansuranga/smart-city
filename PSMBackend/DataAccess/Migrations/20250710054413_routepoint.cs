using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PSMDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class routepoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Route",
                table: "Route");

            migrationBuilder.RenameTable(
                name: "Route",
                newName: "Routes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Routes",
                table: "Routes",
                column: "RouteNo");

            migrationBuilder.CreateTable(
                name: "RoutePoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    RouteNo = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoutePoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoutePoints_Routes_RouteNo",
                        column: x => x.RouteNo,
                        principalTable: "Routes",
                        principalColumn: "RouteNo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "RoutePoints",
                columns: new[] { "Id", "Latitude", "Longitude", "Name", "RouteNo" },
                values: new object[,]
                {
                    { 1, 80.026488999999998, 7.0249980000000001, "maduma bankara vidyalaya", "R001" },
                    { 2, 80.022924000000003, 7.0254649999999996, "gala gawa", "R001" },
                    { 3, 80.021614, 7.0225910000000002, "Sewa Piyasa", "R001" },
                    { 4, 80.023321999999993, 7.0228789999999996, "jayamal oil", "R001" },
                    { 5, 80.026511999999997, 7.0212519999999996, "bathiya home", "R001" },
                    { 6, 80.026774000000003, 7.0229679999999997, "grace health", "R001" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoutePoints_RouteNo",
                table: "RoutePoints",
                column: "RouteNo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoutePoints");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Routes",
                table: "Routes");

            migrationBuilder.RenameTable(
                name: "Routes",
                newName: "Route");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Route",
                table: "Route",
                column: "RouteNo");
        }
    }
}
