using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCo.$safeprojectname$.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Microservices");

            migrationBuilder.CreateTable(
                name: "Associate",
                schema: "Microservices",
                columns: table => new
                {
                    RowKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartitionKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssociateKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Associate", x => x.RowKey);
                });

            migrationBuilder.CreateTable(
                name: "Business",
                schema: "Microservices",
                columns: table => new
                {
                    RowKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartitionKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusinessKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TaxNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Business", x => x.RowKey);
                });

            migrationBuilder.CreateTable(
                name: "Gender",
                schema: "Microservices",
                columns: table => new
                {
                    RowKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartitionKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GenderKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GenderName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GenderCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gender", x => x.RowKey);
                    table.CheckConstraint("CC_Gender_GenderCode", "GenderCode in ('M', 'F', 'N/A', 'U/K')");
                });

            migrationBuilder.CreateTable(
                name: "Government",
                schema: "Microservices",
                columns: table => new
                {
                    RowKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartitionKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GovernmentKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GovernmentName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Government", x => x.RowKey);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                schema: "Microservices",
                columns: table => new
                {
                    RowKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartitionKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    GenderCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.RowKey);
                    table.CheckConstraint("CC_Person_GenderCode", "GenderCode in ('M', 'F', 'N/A', 'U/K')");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssociateLocation_Associate",
                schema: "Microservices",
                table: "Associate",
                column: "AssociateKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Business_Key",
                schema: "Microservices",
                table: "Business",
                column: "BusinessKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gender_Code",
                schema: "Microservices",
                table: "Gender",
                column: "GenderCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gender_Key",
                schema: "Microservices",
                table: "Gender",
                column: "GenderKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Government_Associate",
                schema: "Microservices",
                table: "Government",
                column: "GovernmentKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Person_All",
                schema: "Microservices",
                table: "Person",
                columns: new[] { "FirstName", "MiddleName", "LastName", "BirthDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Person_Associate",
                schema: "Microservices",
                table: "Person",
                column: "PersonKey",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Associate",
                schema: "Microservices");

            migrationBuilder.DropTable(
                name: "Business",
                schema: "Microservices");

            migrationBuilder.DropTable(
                name: "Gender",
                schema: "Microservices");

            migrationBuilder.DropTable(
                name: "Government",
                schema: "Microservices");

            migrationBuilder.DropTable(
                name: "Person",
                schema: "Microservices");
        }
    }
}
