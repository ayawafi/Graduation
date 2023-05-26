using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic_DbModel.Migrations
{
    public partial class addupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "blog",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "blog");
        }
    }
}
