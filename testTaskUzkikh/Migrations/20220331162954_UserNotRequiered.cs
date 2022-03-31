using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace testTaskUzkikh.Migrations
{
    public partial class UserNotRequiered : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Unps_Users_UserId",
                table: "Unps");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "Unps",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Unps_Users_UserId",
                table: "Unps",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Unps_Users_UserId",
                table: "Unps");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "Unps",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Unps_Users_UserId",
                table: "Unps",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
