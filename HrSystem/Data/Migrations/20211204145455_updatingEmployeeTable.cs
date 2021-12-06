using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrSystem.Data.Migrations
{
    public partial class updatingEmployeeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StreetName",
                table: "Employees",
                newName: "SSN");

            migrationBuilder.RenameColumn(
                name: "GovernorateName",
                table: "Employees",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "CityName",
                table: "Employees",
                newName: "Nationality");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "SSN",
                table: "Employees",
                newName: "StreetName");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "Employees",
                newName: "GovernorateName");

            migrationBuilder.RenameColumn(
                name: "Nationality",
                table: "Employees",
                newName: "CityName");
        }
    }
}
