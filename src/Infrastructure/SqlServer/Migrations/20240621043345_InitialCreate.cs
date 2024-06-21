using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Goodtocode.SemanticKernel.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Key = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartitionKey = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "ChatSessions",
                columns: table => new
                {
                    Key = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PartitionKey = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatSessions", x => x.Key);
                    table.ForeignKey(
                        name: "FK_ChatSessions_Authors_AuthorKey",
                        column: x => x.AuthorKey,
                        principalTable: "Authors",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    Key = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChatSessionKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PartitionKey = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Key);
                    table.ForeignKey(
                        name: "FK_ChatMessages_ChatSessions_ChatSessionKey",
                        column: x => x.ChatSessionKey,
                        principalTable: "ChatSessions",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ChatSessionKey",
                table: "ChatMessages",
                column: "ChatSessionKey");

            migrationBuilder.CreateIndex(
                name: "IX_ChatSessions_AuthorKey",
                table: "ChatSessions",
                column: "AuthorKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "ChatSessions");

            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}
