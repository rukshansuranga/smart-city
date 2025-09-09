using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSMDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectCoordinatorRemoveResponsibilityAddCoordinatorType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Responsibility",
                table: "ProjectCoordinators");

            migrationBuilder.AddColumn<int>(
                name: "CoordinatorType",
                table: "ProjectCoordinators",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "ProjectCoordinators",
                keyColumn: "ProjectCoordinatorId",
                keyValue: 1,
                column: "CoordinatorType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ProjectCoordinators",
                keyColumn: "ProjectCoordinatorId",
                keyValue: 2,
                column: "CoordinatorType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ProjectCoordinators",
                keyColumn: "ProjectCoordinatorId",
                keyValue: 3,
                column: "CoordinatorType",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoordinatorType",
                table: "ProjectCoordinators");

            migrationBuilder.AddColumn<string>(
                name: "Responsibility",
                table: "ProjectCoordinators",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ProjectCoordinators",
                keyColumn: "ProjectCoordinatorId",
                keyValue: 1,
                column: "Responsibility",
                value: "Inspect the project");

            migrationBuilder.UpdateData(
                table: "ProjectCoordinators",
                keyColumn: "ProjectCoordinatorId",
                keyValue: 2,
                column: "Responsibility",
                value: "Support");

            migrationBuilder.UpdateData(
                table: "ProjectCoordinators",
                keyColumn: "ProjectCoordinatorId",
                keyValue: 3,
                column: "Responsibility",
                value: "Manage");
        }
    }
}
