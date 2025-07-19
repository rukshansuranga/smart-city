using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PSMDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class tender : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Tender_AwadedTenderId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Tender_Companies_CompanyId",
                table: "Tender");

            migrationBuilder.DropForeignKey(
                name: "FK_Tender_Projects_ProjectId",
                table: "Tender");

            migrationBuilder.DropIndex(
                name: "IX_Projects_AwadedTenderId",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tender",
                table: "Tender");

            migrationBuilder.RenameTable(
                name: "Tender",
                newName: "Tenders");

            migrationBuilder.RenameIndex(
                name: "IX_Tender_ProjectId",
                table: "Tenders",
                newName: "IX_Tenders_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Tender_CompanyId",
                table: "Tenders",
                newName: "IX_Tenders_CompanyId");

            migrationBuilder.AddColumn<LocalDateTime>(
                name: "SubmittedDate",
                table: "Tenders",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new NodaTime.LocalDateTime(1, 1, 1, 0, 0));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tenders",
                table: "Tenders",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Tenders",
                columns: new[] { "Id", "BidAmount", "CompanyId", "Name", "Note", "ProjectId", "SubmittedDate" },
                values: new object[,]
                {
                    { 1, 0m, 1, "Tender for Project A", "This is a tender for Project A", 1, new NodaTime.LocalDateTime(2023, 10, 1, 10, 0) },
                    { 2, 0m, 1, "Tender for Road Construction", "This is a tender for Project A", 1, new NodaTime.LocalDateTime(2023, 10, 1, 10, 0) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_AwadedTenderId",
                table: "Projects",
                column: "AwadedTenderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Tenders_AwadedTenderId",
                table: "Projects",
                column: "AwadedTenderId",
                principalTable: "Tenders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tenders_Companies_CompanyId",
                table: "Tenders",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tenders_Projects_ProjectId",
                table: "Tenders",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Tenders_AwadedTenderId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenders_Companies_CompanyId",
                table: "Tenders");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenders_Projects_ProjectId",
                table: "Tenders");

            migrationBuilder.DropIndex(
                name: "IX_Projects_AwadedTenderId",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tenders",
                table: "Tenders");

            migrationBuilder.DeleteData(
                table: "Tenders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tenders",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "SubmittedDate",
                table: "Tenders");

            migrationBuilder.RenameTable(
                name: "Tenders",
                newName: "Tender");

            migrationBuilder.RenameIndex(
                name: "IX_Tenders_ProjectId",
                table: "Tender",
                newName: "IX_Tender_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Tenders_CompanyId",
                table: "Tender",
                newName: "IX_Tender_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tender",
                table: "Tender",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_AwadedTenderId",
                table: "Projects",
                column: "AwadedTenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Tender_AwadedTenderId",
                table: "Projects",
                column: "AwadedTenderId",
                principalTable: "Tender",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tender_Companies_CompanyId",
                table: "Tender",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tender_Projects_ProjectId",
                table: "Tender",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
