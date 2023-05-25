using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic_DbModel.Migrations
{
    public partial class addupdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Time",
                table: "appointment",
                newName: "StartTime");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Users",
                type: "longtext",
                nullable: true);

            migrationBuilder.AlterColumn<sbyte>(
                name: "IsDeleted",
                table: "appointment",
                type: "tinyint",
                nullable: false,
                defaultValueSql: "'0'",
                oldClrType: typeof(sbyte),
                oldType: "tinyint",
                oldNullable: true,
                oldDefaultValueSql: "'0'");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "appointment",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "appointment");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "appointment",
                newName: "Time");

            migrationBuilder.AlterColumn<sbyte>(
                name: "IsDeleted",
                table: "appointment",
                type: "tinyint",
                nullable: true,
                defaultValueSql: "'0'",
                oldClrType: typeof(sbyte),
                oldType: "tinyint",
                oldDefaultValueSql: "'0'");
        }
    }
}
