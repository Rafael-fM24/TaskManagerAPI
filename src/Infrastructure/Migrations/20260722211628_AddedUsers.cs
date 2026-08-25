using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskNote_TaskItem_TaskItemId",
                table: "TaskNote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskNote",
                table: "TaskNote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskItem",
                table: "TaskItem");

            migrationBuilder.RenameTable(
                name: "TaskNote",
                newName: "TaskNotes");

            migrationBuilder.RenameTable(
                name: "TaskItem",
                newName: "TaskItems");

            migrationBuilder.RenameIndex(
                name: "IX_TaskNote_TaskItemId",
                table: "TaskNotes",
                newName: "IX_TaskNotes_TaskItemId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DueDate",
                table: "TaskItems",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "TaskItems",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskNotes",
                table: "TaskNotes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskItems",
                table: "TaskItems",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Username = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_UserId",
                table: "TaskItems",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItems_Users_UserId",
                table: "TaskItems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskNotes_TaskItems_TaskItemId",
                table: "TaskNotes",
                column: "TaskItemId",
                principalTable: "TaskItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskItems_Users_UserId",
                table: "TaskItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskNotes_TaskItems_TaskItemId",
                table: "TaskNotes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskNotes",
                table: "TaskNotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskItems",
                table: "TaskItems");

            migrationBuilder.DropIndex(
                name: "IX_TaskItems_UserId",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TaskItems");

            migrationBuilder.RenameTable(
                name: "TaskNotes",
                newName: "TaskNote");

            migrationBuilder.RenameTable(
                name: "TaskItems",
                newName: "TaskItem");

            migrationBuilder.RenameIndex(
                name: "IX_TaskNotes_TaskItemId",
                table: "TaskNote",
                newName: "IX_TaskNote_TaskItemId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DueDate",
                table: "TaskItem",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskNote",
                table: "TaskNote",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskItem",
                table: "TaskItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskNote_TaskItem_TaskItemId",
                table: "TaskNote",
                column: "TaskItemId",
                principalTable: "TaskItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
