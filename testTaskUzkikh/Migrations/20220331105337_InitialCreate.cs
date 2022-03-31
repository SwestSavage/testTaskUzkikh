using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace testTaskUzkikh.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Unps",
                columns: table => new
                {
                    UnpId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VUNP = table.Column<long>(type: "bigint", nullable: false),
                    VNAIMP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VNAIMK = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DREG = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NMNS = table.Column<long>(type: "bigint", nullable: false),
                    VMNS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CKODSOST = table.Column<int>(type: "int", nullable: false),
                    VKODS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DLIKV = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VLIKV = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unps", x => x.UnpId);
                    table.ForeignKey(
                        name: "FK_Unps_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Unps_UserId",
                table: "Unps",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Unps");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
