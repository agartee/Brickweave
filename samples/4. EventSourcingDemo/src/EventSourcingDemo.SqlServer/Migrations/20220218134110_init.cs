using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventSourcingDemo.SqlServer.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "EventSourcingDemo");

            migrationBuilder.CreateTable(
                name: "Company",
                schema: "EventSourcingDemo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                schema: "EventSourcingDemo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StreamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CommitSequence = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessageOutbox",
                schema: "EventSourcingDemo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CommitSequence = table.Column<int>(type: "int", nullable: false),
                    IsSending = table.Column<bool>(type: "bit", nullable: false),
                    SendAttemptCount = table.Column<int>(type: "int", nullable: false),
                    LastSendAttempt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageOutbox", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                schema: "EventSourcingDemo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BusinessAccount",
                schema: "EventSourcingDemo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AccountHolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessAccount_Company_AccountHolderId",
                        column: x => x.AccountHolderId,
                        principalSchema: "EventSourcingDemo",
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonalAccount",
                schema: "EventSourcingDemo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AccountHolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalAccount_Person_AccountHolderId",
                        column: x => x.AccountHolderId,
                        principalSchema: "EventSourcingDemo",
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessAccount_AccountHolderId",
                schema: "EventSourcingDemo",
                table: "BusinessAccount",
                column: "AccountHolderId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalAccount_AccountHolderId",
                schema: "EventSourcingDemo",
                table: "PersonalAccount",
                column: "AccountHolderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessAccount",
                schema: "EventSourcingDemo");

            migrationBuilder.DropTable(
                name: "Event",
                schema: "EventSourcingDemo");

            migrationBuilder.DropTable(
                name: "MessageOutbox",
                schema: "EventSourcingDemo");

            migrationBuilder.DropTable(
                name: "PersonalAccount",
                schema: "EventSourcingDemo");

            migrationBuilder.DropTable(
                name: "Company",
                schema: "EventSourcingDemo");

            migrationBuilder.DropTable(
                name: "Person",
                schema: "EventSourcingDemo");
        }
    }
}
