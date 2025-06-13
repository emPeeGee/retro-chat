using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Retro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedEmojiReactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageReaction_EmojiReaction_EmojiReactionId",
                table: "MessageReaction");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageReaction_Messages_MessageId",
                table: "MessageReaction");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageReaction_Users_UserId",
                table: "MessageReaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageReaction",
                table: "MessageReaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmojiReaction",
                table: "EmojiReaction");

            migrationBuilder.RenameTable(
                name: "MessageReaction",
                newName: "MessageReactions");

            migrationBuilder.RenameTable(
                name: "EmojiReaction",
                newName: "EmojiReactions");

            migrationBuilder.RenameIndex(
                name: "IX_MessageReaction_UserId",
                table: "MessageReactions",
                newName: "IX_MessageReactions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MessageReaction_MessageId",
                table: "MessageReactions",
                newName: "IX_MessageReactions_MessageId");

            migrationBuilder.RenameIndex(
                name: "IX_MessageReaction_EmojiReactionId",
                table: "MessageReactions",
                newName: "IX_MessageReactions_EmojiReactionId");

            migrationBuilder.RenameIndex(
                name: "IX_EmojiReaction_Name",
                table: "EmojiReactions",
                newName: "IX_EmojiReactions_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageReactions",
                table: "MessageReactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmojiReactions",
                table: "EmojiReactions",
                column: "Id");

            migrationBuilder.InsertData(
                table: "EmojiReactions",
                columns: new[] { "Id", "Emoji", "Name" },
                values: new object[,]
                {
                    { 1, "👍", "Like" },
                    { 2, "❤️", "Love" },
                    { 3, "😂", "Laugh" },
                    { 4, "😮", "Surprised" },
                    { 5, "😡", "Angry" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_MessageReactions_EmojiReactions_EmojiReactionId",
                table: "MessageReactions",
                column: "EmojiReactionId",
                principalTable: "EmojiReactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageReactions_Messages_MessageId",
                table: "MessageReactions",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageReactions_Users_UserId",
                table: "MessageReactions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageReactions_EmojiReactions_EmojiReactionId",
                table: "MessageReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageReactions_Messages_MessageId",
                table: "MessageReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageReactions_Users_UserId",
                table: "MessageReactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageReactions",
                table: "MessageReactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmojiReactions",
                table: "EmojiReactions");

            migrationBuilder.DeleteData(
                table: "EmojiReactions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EmojiReactions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EmojiReactions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "EmojiReactions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "EmojiReactions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.RenameTable(
                name: "MessageReactions",
                newName: "MessageReaction");

            migrationBuilder.RenameTable(
                name: "EmojiReactions",
                newName: "EmojiReaction");

            migrationBuilder.RenameIndex(
                name: "IX_MessageReactions_UserId",
                table: "MessageReaction",
                newName: "IX_MessageReaction_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MessageReactions_MessageId",
                table: "MessageReaction",
                newName: "IX_MessageReaction_MessageId");

            migrationBuilder.RenameIndex(
                name: "IX_MessageReactions_EmojiReactionId",
                table: "MessageReaction",
                newName: "IX_MessageReaction_EmojiReactionId");

            migrationBuilder.RenameIndex(
                name: "IX_EmojiReactions_Name",
                table: "EmojiReaction",
                newName: "IX_EmojiReaction_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageReaction",
                table: "MessageReaction",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmojiReaction",
                table: "EmojiReaction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageReaction_EmojiReaction_EmojiReactionId",
                table: "MessageReaction",
                column: "EmojiReactionId",
                principalTable: "EmojiReaction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageReaction_Messages_MessageId",
                table: "MessageReaction",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageReaction_Users_UserId",
                table: "MessageReaction",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
