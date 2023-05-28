using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic_DbModel.Migrations
{
    public partial class addlatestupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_doctor_Users_ApplicationUserId",
                table: "doctor");

            migrationBuilder.DropIndex(
                name: "IX_doctor_ApplicationUserId",
                table: "doctor");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "doctor");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "doctor",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_doctor_UserId",
                table: "doctor",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_doctor_Users_UserId",
                table: "doctor",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_doctor_Users_UserId",
                table: "doctor");

            migrationBuilder.DropIndex(
                name: "IX_doctor_UserId",
                table: "doctor");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "doctor",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "doctor",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_doctor_ApplicationUserId",
                table: "doctor",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_doctor_Users_ApplicationUserId",
                table: "doctor",
                column: "ApplicationUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
