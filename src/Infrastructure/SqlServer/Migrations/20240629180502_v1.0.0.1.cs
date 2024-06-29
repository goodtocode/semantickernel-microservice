using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Goodtocode.SemanticKernel.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class v11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartitionKey",
                table: "ChatSessions");

            migrationBuilder.DropColumn(
                name: "PartitionKey",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "PartitionKey",
                table: "Authors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PartitionKey",
                table: "ChatSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PartitionKey",
                table: "ChatMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PartitionKey",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
