using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSMDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RefactorCommentEntityComplete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Clear existing comment data before schema changes
            migrationBuilder.Sql("DELETE FROM \"Comments\"");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Clients_ClientId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Complains_ComplainId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ComplainId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ComplainId",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Comments",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Comments",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrivate",
                table: "Comments",
                type: "boolean",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EntityId",
                table: "Comments",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EntityType",
                table: "Comments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_EntityType_EntityId",
                table: "Comments",
                columns: new[] { "EntityType", "EntityId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Clients_ClientId",
                table: "Comments",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Clear existing comment data before reverting schema changes
            migrationBuilder.Sql("DELETE FROM \"Comments\"");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Clients_ClientId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_EntityType_EntityId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Comments",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Comments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrivate",
                table: "Comments",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ComplainId",
                table: "Comments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ComplainId",
                table: "Comments",
                column: "ComplainId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Clients_ClientId",
                table: "Comments",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Complains_ComplainId",
                table: "Comments",
                column: "ComplainId",
                principalTable: "Complains",
                principalColumn: "ComplainId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
