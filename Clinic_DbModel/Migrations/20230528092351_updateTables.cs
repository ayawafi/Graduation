using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic_DbModel.Migrations
{
    public partial class updateTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExperienceTo",
                table: "doctor",
                newName: "HospitalTo");

            migrationBuilder.RenameColumn(
                name: "ExperienceFrom",
                table: "doctor",
                newName: "HospitalFrom");

            migrationBuilder.RenameColumn(
                name: "Experience",
                table: "doctor",
                newName: "HospitalName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HospitalTo",
                table: "doctor",
                newName: "ExperienceTo");

            migrationBuilder.RenameColumn(
                name: "HospitalName",
                table: "doctor",
                newName: "Experience");

            migrationBuilder.RenameColumn(
                name: "HospitalFrom",
                table: "doctor",
                newName: "ExperienceFrom");
        }
    }
}
