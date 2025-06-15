using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMessageStatusesEditTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageStatus_Messages_MessageId",
                table: "MessageStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageStatus_Users_UserId",
                table: "MessageStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageStatus",
                table: "MessageStatus");

            migrationBuilder.RenameTable(
                name: "MessageStatus",
                newName: "MessageStatuses");

            migrationBuilder.RenameIndex(
                name: "IX_MessageStatus_UserId",
                table: "MessageStatuses",
                newName: "IX_MessageStatuses_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageStatuses",
                table: "MessageStatuses",
                columns: new[] { "MessageId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MessageStatuses_Messages_MessageId",
                table: "MessageStatuses",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageStatuses_Users_UserId",
                table: "MessageStatuses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageStatuses_Messages_MessageId",
                table: "MessageStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageStatuses_Users_UserId",
                table: "MessageStatuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageStatuses",
                table: "MessageStatuses");

            migrationBuilder.RenameTable(
                name: "MessageStatuses",
                newName: "MessageStatus");

            migrationBuilder.RenameIndex(
                name: "IX_MessageStatuses_UserId",
                table: "MessageStatus",
                newName: "IX_MessageStatus_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageStatus",
                table: "MessageStatus",
                columns: new[] { "MessageId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MessageStatus_Messages_MessageId",
                table: "MessageStatus",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageStatus_Users_UserId",
                table: "MessageStatus",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
