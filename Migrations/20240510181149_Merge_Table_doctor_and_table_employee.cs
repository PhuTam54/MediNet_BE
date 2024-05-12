using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediNet_BE.Migrations
{
    /// <inheritdoc />
    public partial class Merge_Table_doctor_and_table_employee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Doctors_DoctorId",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Doctors_DoctorId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Clinics_ClinicId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Specialists_SpecialistId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Doctors_DoctorId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "Services",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Services_DoctorId",
                table: "Services",
                newName: "IX_Services_EmployeeId");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "Courses",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_DoctorId",
                table: "Courses",
                newName: "IX_Courses_EmployeeId");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "Blogs",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Blogs_DoctorId",
                table: "Blogs",
                newName: "IX_Blogs_EmployeeId");

            migrationBuilder.AddColumn<int>(
                name: "PositionId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Employees_EmployeeId",
                table: "Blogs",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Employees_EmployeeId",
                table: "Courses",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Clinics_ClinicId",
                table: "Employees",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Specialists_SpecialistId",
                table: "Employees",
                column: "SpecialistId",
                principalTable: "Specialists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Employees_EmployeeId",
                table: "Services",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Employees_EmployeeId",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Employees_EmployeeId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Clinics_ClinicId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Specialists_SpecialistId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Employees_EmployeeId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Services",
                newName: "DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_Services_EmployeeId",
                table: "Services",
                newName: "IX_Services_DoctorId");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Courses",
                newName: "DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_EmployeeId",
                table: "Courses",
                newName: "IX_Courses_DoctorId");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Blogs",
                newName: "DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_Blogs_EmployeeId",
                table: "Blogs",
                newName: "IX_Blogs_DoctorId");

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClinicId = table.Column<int>(type: "int", nullable: false),
                    SpecialistId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date_Of_Birth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Full_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PositionId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    SEO_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Doctors_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Doctors_Specialists_SpecialistId",
                        column: x => x.SpecialistId,
                        principalTable: "Specialists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_ClinicId",
                table: "Doctors",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_SpecialistId",
                table: "Doctors",
                column: "SpecialistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Doctors_DoctorId",
                table: "Blogs",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Doctors_DoctorId",
                table: "Courses",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Clinics_ClinicId",
                table: "Employees",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Specialists_SpecialistId",
                table: "Employees",
                column: "SpecialistId",
                principalTable: "Specialists",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Doctors_DoctorId",
                table: "Services",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
