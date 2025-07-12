using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace PSMDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class rideseed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Rides",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<LocalDateTime>(
                name: "EndTime",
                table: "Rides",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(LocalDateTime),
                oldType: "timestamp without time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Rides",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<LocalDateTime>(
                name: "EndTime",
                table: "Rides",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new NodaTime.LocalDateTime(1, 1, 1, 0, 0),
                oldClrType: typeof(LocalDateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);
        }
    }
}
