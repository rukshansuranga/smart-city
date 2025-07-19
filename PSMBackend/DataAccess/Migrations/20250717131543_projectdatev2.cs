using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace PSMDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class projectdatev2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<LocalDate>(
                name: "TenderOpeningDate",
                table: "Projects",
                type: "date",
                nullable: true,
                oldClrType: typeof(LocalDateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<LocalDate>(
                name: "TenderClosingDate",
                table: "Projects",
                type: "date",
                nullable: true,
                oldClrType: typeof(LocalDateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "TenderClosingDate", "TenderOpeningDate" },
                values: new object[] { new NodaTime.LocalDate(2023, 1, 30), new NodaTime.LocalDate(2023, 1, 15) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "TenderClosingDate", "TenderOpeningDate" },
                values: new object[] { new NodaTime.LocalDate(2023, 1, 30), new NodaTime.LocalDate(2023, 1, 15) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<LocalDateTime>(
                name: "TenderOpeningDate",
                table: "Projects",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(LocalDate),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<LocalDateTime>(
                name: "TenderClosingDate",
                table: "Projects",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(LocalDate),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "TenderClosingDate", "TenderOpeningDate" },
                values: new object[] { new NodaTime.LocalDateTime(2023, 1, 30, 17, 0), new NodaTime.LocalDateTime(2023, 1, 15, 10, 0) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "TenderClosingDate", "TenderOpeningDate" },
                values: new object[] { new NodaTime.LocalDateTime(2023, 1, 30, 17, 0), new NodaTime.LocalDateTime(2023, 1, 15, 10, 0) });
        }
    }
}
