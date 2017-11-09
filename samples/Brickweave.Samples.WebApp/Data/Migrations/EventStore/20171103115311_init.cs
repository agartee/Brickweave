using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Brickweave.Samples.WebApp.Data.Migrations.EventStore
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "EventStore");

            migrationBuilder.CreateTable(
                name: "Event",
                schema: "EventStore",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommitSequence = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StreamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.EventId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Event",
                schema: "EventStore");
        }
    }
}
