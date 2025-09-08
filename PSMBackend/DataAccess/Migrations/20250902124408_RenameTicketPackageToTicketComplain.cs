using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PSMDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RenameTicketPackageToTicketComplain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketPackages");

            migrationBuilder.CreateTable(
                name: "TicketComplains",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TicketId = table.Column<int>(type: "integer", nullable: false),
                    ComplainId = table.Column<int>(type: "integer", nullable: false),
                    ComplainTicketTicketId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketComplains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketComplains_Complains_ComplainId",
                        column: x => x.ComplainId,
                        principalTable: "Complains",
                        principalColumn: "ComplainId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketComplains_Tickets_ComplainTicketTicketId",
                        column: x => x.ComplainTicketTicketId,
                        principalTable: "Tickets",
                        principalColumn: "TicketId");
                    table.ForeignKey(
                        name: "FK_TicketComplains_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TicketComplains",
                columns: new[] { "Id", "ComplainId", "ComplainTicketTicketId", "TicketId" },
                values: new object[,]
                {
                    { 1, 2, null, 1 },
                    { 2, 3, null, 1 },
                    { 3, 4, null, 3 },
                    { 4, 5, null, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketComplains_ComplainId",
                table: "TicketComplains",
                column: "ComplainId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketComplains_ComplainTicketTicketId",
                table: "TicketComplains",
                column: "ComplainTicketTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketComplains_TicketId",
                table: "TicketComplains",
                column: "TicketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketComplains");

            migrationBuilder.CreateTable(
                name: "TicketPackages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ComplainId = table.Column<int>(type: "integer", nullable: false),
                    TicketId = table.Column<int>(type: "integer", nullable: false),
                    ComplainTicketTicketId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketPackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketPackages_Complains_ComplainId",
                        column: x => x.ComplainId,
                        principalTable: "Complains",
                        principalColumn: "ComplainId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketPackages_Tickets_ComplainTicketTicketId",
                        column: x => x.ComplainTicketTicketId,
                        principalTable: "Tickets",
                        principalColumn: "TicketId");
                    table.ForeignKey(
                        name: "FK_TicketPackages_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TicketPackages",
                columns: new[] { "Id", "ComplainId", "ComplainTicketTicketId", "TicketId" },
                values: new object[,]
                {
                    { 1, 2, null, 1 },
                    { 2, 3, null, 1 },
                    { 3, 4, null, 3 },
                    { 4, 5, null, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketPackages_ComplainId",
                table: "TicketPackages",
                column: "ComplainId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketPackages_ComplainTicketTicketId",
                table: "TicketPackages",
                column: "ComplainTicketTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketPackages_TicketId",
                table: "TicketPackages",
                column: "TicketId");
        }
    }
}
