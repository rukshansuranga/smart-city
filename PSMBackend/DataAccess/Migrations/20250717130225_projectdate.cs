using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace PSMDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class projectdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<LocalDate>(
                name: "StartDate",
                table: "Projects",
                type: "date",
                nullable: true,
                oldClrType: typeof(LocalDateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<LocalDate>(
                name: "EndDate",
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
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new NodaTime.LocalDate(2023, 12, 31), new NodaTime.LocalDate(2023, 1, 1) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new NodaTime.LocalDate(2023, 12, 31), new NodaTime.LocalDate(2023, 1, 1) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<LocalDateTime>(
                name: "StartDate",
                table: "Projects",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(LocalDate),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<LocalDateTime>(
                name: "EndDate",
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
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new NodaTime.LocalDateTime(2023, 12, 31, 23, 59), new NodaTime.LocalDateTime(2023, 1, 1, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new NodaTime.LocalDateTime(2023, 12, 31, 23, 59), new NodaTime.LocalDateTime(2023, 1, 1, 0, 0) });
        }
    }
}
