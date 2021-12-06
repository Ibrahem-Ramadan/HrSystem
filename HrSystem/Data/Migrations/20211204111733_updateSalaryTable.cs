using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrSystem.Data.Migrations
{
    public partial class updateSalaryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SalaryAmount",
                table: "Salaries",
                newName: "TotalSalary");

            migrationBuilder.AddColumn<double>(
                name: "SalaryAmount",
                table: "Employees",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalaryAmount",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "TotalSalary",
                table: "Salaries",
                newName: "SalaryAmount");
        }
    }
}
