using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AdvancedCqrs.SqlServer.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "AdvancedCqrs");

            migrationBuilder.CreateTable(
                name: "CommandQueue",
                schema: "AdvancedCqrs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CommandJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimsPrincipalJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsProcessing = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandQueue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommandStatus",
                schema: "AdvancedCqrs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Thing",
                schema: "AdvancedCqrs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Thing", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommandQueue",
                schema: "AdvancedCqrs");

            migrationBuilder.DropTable(
                name: "CommandStatus",
                schema: "AdvancedCqrs");

            migrationBuilder.DropTable(
                name: "Thing",
                schema: "AdvancedCqrs");
        }
    }
}
