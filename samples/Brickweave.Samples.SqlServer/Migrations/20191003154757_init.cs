using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Brickweave.Samples.SqlServer.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StreamId = table.Column<Guid>(nullable: false),
                    Json = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    CommitSequence = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessageOutbox",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Json = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    CommitSequence = table.Column<int>(nullable: false),
                    IsSending = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageOutbox", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Json = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "MessageOutbox");

            migrationBuilder.DropTable(
                name: "Person");
        }
    }
}
