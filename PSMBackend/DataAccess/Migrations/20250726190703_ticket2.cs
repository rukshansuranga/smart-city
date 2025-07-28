using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSMDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ticket2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Tenders_AwadedTenderId",
                table: "Projects");

            // migrationBuilder.DropIndex(
            //     name: "IX_Tenders_ProjectId",
            //     table: "Tenders");

            // migrationBuilder.DropIndex(
            //     name: "IX_Projects_AwadedTenderId",
            //     table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Tickets",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Tickets",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "TicketId",
                keyValue: 1,
                columns: new[] { "Status", "Type" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "TicketId",
                keyValue: 2,
                columns: new[] { "Status", "Type" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "TicketId",
                keyValue: 3,
                columns: new[] { "Status", "Type" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "TicketId",
                keyValue: 4,
                columns: new[] { "Status", "Type" },
                values: new object[] { null, null });

            // migrationBuilder.CreateIndex(
            //     name: "IX_Tenders_ProjectId",
            //     table: "Tenders",
            //     column: "ProjectId",
            //     unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tenders_ProjectId",
                table: "Tenders");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Tickets");

            migrationBuilder.CreateIndex(
                name: "IX_Tenders_ProjectId",
                table: "Tenders",
                column: "ProjectId");

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
        }
    }
}
