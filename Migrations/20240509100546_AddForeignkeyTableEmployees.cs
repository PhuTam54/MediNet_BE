using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediNet_BE.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignkeyTableEmployees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClinicId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SpecialistId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ClinicId",
                table: "Employees",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SpecialistId",
                table: "Employees",
                column: "SpecialistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Clinics_ClinicId",
                table: "Employees",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Specialists_SpecialistId",
                table: "Employees",
                column: "SpecialistId",
                principalTable: "Specialists",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Clinics_ClinicId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Specialists_SpecialistId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ClinicId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_SpecialistId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ClinicId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "SpecialistId",
                table: "Employees");
        }
    }
}
