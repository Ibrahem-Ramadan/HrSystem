using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrSystem.Data.Migrations
{
    public partial class addEmployeeVacationRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "Vacations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vacations_EmployeeId",
                table: "Vacations",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vacations_Employees_EmployeeId",
                table: "Vacations",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vacations_Employees_EmployeeId",
                table: "Vacations");

            migrationBuilder.DropIndex(
                name: "IX_Vacations_EmployeeId",
                table: "Vacations");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Vacations");
        }
    }
}
