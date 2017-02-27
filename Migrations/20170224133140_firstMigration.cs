using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyStarwarsApi.Migrations
{
    public partial class firstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    Characterid = table.Column<Guid>(nullable: true),
                    name = table.Column<string>(nullable: false),
                    side = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.id);
                    table.ForeignKey(
                        name: "FK_Characters_Characters_Characterid",
                        column: x => x.Characterid,
                        principalTable: "Characters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characters_Characterid",
                table: "Characters",
                column: "Characterid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Characters");
        }
    }
}
