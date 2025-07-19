using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PSMDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class project : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    SpecificationDocument = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    EndDate = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: true),
                    LocationNote = table.Column<string>(type: "text", nullable: true),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    EstimatedCost = table.Column<decimal>(type: "numeric", nullable: false),
                    AwadedTenderId = table.Column<int>(type: "integer", nullable: true),
                    TenderOpeningDate = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    TenderClosingDate = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tender",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: false),
                    BidAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tender", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tender_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tender_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "AwadedTenderId", "Description", "EndDate", "EstimatedCost", "Latitude", "Location", "LocationNote", "Longitude", "Name", "SpecificationDocument", "StartDate", "Status", "TenderClosingDate", "TenderOpeningDate", "Type" },
                values: new object[,]
                {
                    { 1, null, "Description for Project Alpha", new NodaTime.LocalDateTime(2023, 12, 31, 23, 59), 1000000m, 34.052199999999999, "Location A", "Note for Location A", -118.2437, "Project Alpha", "specification_alpha.pdf", new NodaTime.LocalDateTime(2023, 1, 1, 0, 0), "Proposed", new NodaTime.LocalDateTime(2023, 1, 30, 17, 0), new NodaTime.LocalDateTime(2023, 1, 15, 10, 0), "Construction" },
                    { 2, null, "Weliveriya road project", new NodaTime.LocalDateTime(2023, 12, 31, 23, 59), 1000000m, 34.052199999999999, "Location A", "Note for Location A", -118.2437, "Weliveriya road project", "specification_alpha.pdf", new NodaTime.LocalDateTime(2023, 1, 1, 0, 0), "Proposed", new NodaTime.LocalDateTime(2023, 1, 30, 17, 0), new NodaTime.LocalDateTime(2023, 1, 15, 10, 0), "Road Construction" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_AwadedTenderId",
                table: "Projects",
                column: "AwadedTenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Tender_CompanyId",
                table: "Tender",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tender_ProjectId",
                table: "Tender",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Tender_AwadedTenderId",
                table: "Projects",
                column: "AwadedTenderId",
                principalTable: "Tender",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Tender_AwadedTenderId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "Tender");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
