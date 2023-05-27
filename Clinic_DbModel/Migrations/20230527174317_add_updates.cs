using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic_DbModel.Migrations
{
    public partial class add_updates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "blog");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "specialization",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "specialization");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "blog",
                type: "longtext",
                nullable: true);
        }
    }
}
