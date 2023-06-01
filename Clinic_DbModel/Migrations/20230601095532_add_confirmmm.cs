using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic_DbModel.Migrations
{
    public partial class add_confirmmm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmationLink",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "ConfirmationCode",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmationCode",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "ConfirmationLink",
                table: "Users",
                type: "longtext",
                nullable: true);
        }
    }
}
