using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic_DbModel.Migrations
{
    public partial class FavDoctor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "FavDoctors",
                columns: table => new
                {
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavDoctors", x => new { x.DoctorId, x.ApplicationUserId });
                    table.ForeignKey(
                        name: "FK_FavDoctors_doctor_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "doctor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavDoctors_Users_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_FavDoctors_ApplicationUserId",
                table: "FavDoctors",
                column: "ApplicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavDoctors");
            migrationBuilder.CreateIndex(
                name: "IX_FavouritesDoctor_ApplicationUserId",
                table: "FavouritesDoctor",
                column: "ApplicationUserId");
        }
    }
}
