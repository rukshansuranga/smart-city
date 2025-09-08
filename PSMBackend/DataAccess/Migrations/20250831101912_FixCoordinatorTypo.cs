using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PSMDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixCoordinatorTypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectInspections_ProjectCordinators_ProjectCordinatorId",
                table: "ProjectInspections");

            migrationBuilder.DropTable(
                name: "ProjectCordinators");

            migrationBuilder.RenameColumn(
                name: "ProjectCordinatorId",
                table: "ProjectInspections",
                newName: "ProjectCoordinatorId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectInspections_ProjectCordinatorId",
                table: "ProjectInspections",
                newName: "IX_ProjectInspections_ProjectCoordinatorId");

            migrationBuilder.CreateTable(
                name: "ProjectCoordinators",
                columns: table => new
                {
                    ProjectCoordinatorId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    CoordinatorId = table.Column<string>(type: "text", nullable: false),
                    AssignDate = table.Column<LocalDate>(type: "date", nullable: false),
                    Responsibility = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectCoordinators", x => x.ProjectCoordinatorId);
                    table.ForeignKey(
                        name: "FK_ProjectCoordinators_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectCoordinators_Users_CoordinatorId",
                        column: x => x.CoordinatorId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ProjectCoordinators",
                columns: new[] { "ProjectCoordinatorId", "AssignDate", "CoordinatorId", "CreatedBy", "IsActive", "Note", "ProjectId", "Responsibility", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new NodaTime.LocalDate(2025, 8, 24), "0c895075-b8e6-48f9-bb9e-2c9db9d7207a", null, true, "Can you provide an update on the project status?", 1, "Inspect the project", null, null },
                    { 2, new NodaTime.LocalDate(2025, 8, 24), "43e63068-a5fd-4a45-acfb-0383ff4d45ea", null, true, "Can you give support on the project?", 1, "Support", null, null },
                    { 3, new NodaTime.LocalDate(2025, 8, 24), "6c35c5ad-2f70-4c3f-aa44-c94bc61d10a1", null, true, "Can you give support on the project?", 2, "Manage", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCoordinators_CoordinatorId",
                table: "ProjectCoordinators",
                column: "CoordinatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCoordinators_ProjectId",
                table: "ProjectCoordinators",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectInspections_ProjectCoordinators_ProjectCoordinatorId",
                table: "ProjectInspections",
                column: "ProjectCoordinatorId",
                principalTable: "ProjectCoordinators",
                principalColumn: "ProjectCoordinatorId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectInspections_ProjectCoordinators_ProjectCoordinatorId",
                table: "ProjectInspections");

            migrationBuilder.DropTable(
                name: "ProjectCoordinators");

            migrationBuilder.RenameColumn(
                name: "ProjectCoordinatorId",
                table: "ProjectInspections",
                newName: "ProjectCordinatorId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectInspections_ProjectCoordinatorId",
                table: "ProjectInspections",
                newName: "IX_ProjectInspections_ProjectCordinatorId");

            migrationBuilder.CreateTable(
                name: "ProjectCordinators",
                columns: table => new
                {
                    ProjectCordinatorId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CordinatorId = table.Column<string>(type: "text", nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    AssignDate = table.Column<LocalDate>(type: "date", nullable: false),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Responsibility = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectCordinators", x => x.ProjectCordinatorId);
                    table.ForeignKey(
                        name: "FK_ProjectCordinators_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectCordinators_Users_CordinatorId",
                        column: x => x.CordinatorId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ProjectCordinators",
                columns: new[] { "ProjectCordinatorId", "AssignDate", "CordinatorId", "CreatedBy", "IsActive", "Note", "ProjectId", "Responsibility", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new NodaTime.LocalDate(2025, 8, 24), "0c895075-b8e6-48f9-bb9e-2c9db9d7207a", null, true, "Can you provide an update on the project status?", 1, "Inspect the project", null, null },
                    { 2, new NodaTime.LocalDate(2025, 8, 24), "43e63068-a5fd-4a45-acfb-0383ff4d45ea", null, true, "Can you give support on the project?", 1, "Support", null, null },
                    { 3, new NodaTime.LocalDate(2025, 8, 24), "6c35c5ad-2f70-4c3f-aa44-c94bc61d10a1", null, true, "Can you give support on the project?", 2, "Manage", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCordinators_CordinatorId",
                table: "ProjectCordinators",
                column: "CordinatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCordinators_ProjectId",
                table: "ProjectCordinators",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectInspections_ProjectCordinators_ProjectCordinatorId",
                table: "ProjectInspections",
                column: "ProjectCordinatorId",
                principalTable: "ProjectCordinators",
                principalColumn: "ProjectCordinatorId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
