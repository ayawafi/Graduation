using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic_DbModel.Migrations
{
    public partial class Add_Fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cliniclicense",
                table: "doctor",
                newName: "Registration");

            migrationBuilder.AddColumn<string>(
                name: "Awards",
                table: "doctor",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AwardsYear",
                table: "doctor",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Designation",
                table: "doctor",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Experience",
                table: "doctor",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExperienceFrom",
                table: "doctor",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExperienceTo",
                table: "doctor",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Membership",
                table: "doctor",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pricing",
                table: "doctor",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Awards",
                table: "doctor");

            migrationBuilder.DropColumn(
                name: "AwardsYear",
                table: "doctor");

            migrationBuilder.DropColumn(
                name: "Designation",
                table: "doctor");

            migrationBuilder.DropColumn(
                name: "Experience",
                table: "doctor");

            migrationBuilder.DropColumn(
                name: "ExperienceFrom",
                table: "doctor");

            migrationBuilder.DropColumn(
                name: "ExperienceTo",
                table: "doctor");

            migrationBuilder.DropColumn(
                name: "Membership",
                table: "doctor");

            migrationBuilder.DropColumn(
                name: "Pricing",
                table: "doctor");

            migrationBuilder.RenameColumn(
                name: "Registration",
                table: "doctor",
                newName: "Cliniclicense");
        }
    }
}
