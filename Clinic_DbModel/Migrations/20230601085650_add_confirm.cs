using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic_DbModel.Migrations
{
    public partial class add_confirm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConfirmationLink",
                table: "Users",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmationLink",
                table: "Users");
        }
    }
}
