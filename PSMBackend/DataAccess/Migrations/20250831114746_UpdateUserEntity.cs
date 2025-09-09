using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSMDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthType",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Council",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "0c895075-b8e6-48f9-bb9e-2c9db9d7207a",
                columns: new[] { "AuthType", "Council" },
                values: new object[] { null, "Mahara" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "4",
                columns: new[] { "AuthType", "Council" },
                values: new object[] { null, "Mahara" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "43e63068-a5fd-4a45-acfb-0383ff4d45ea",
                columns: new[] { "AuthType", "Council" },
                values: new object[] { null, "Mahara" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "5",
                columns: new[] { "AuthType", "Council" },
                values: new object[] { null, "Mahara" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "6c35c5ad-2f70-4c3f-aa44-c94bc61d10a1",
                columns: new[] { "AuthType", "Council" },
                values: new object[] { null, "Mahara" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "a4aa5b28-36ab-4991-975a-5e5e441bf6fa",
                columns: new[] { "AuthType", "Council" },
                values: new object[] { null, "Mahara" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "d0eca5fa-8cf4-4256-ab6a-9405c789c1b1",
                columns: new[] { "AuthType", "Council" },
                values: new object[] { null, "Mahara" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthType",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Council",
                table: "Users");
        }
    }
}
