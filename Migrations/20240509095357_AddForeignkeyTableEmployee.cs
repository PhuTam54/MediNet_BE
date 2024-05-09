using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediNet_BE.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignkeyTableEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Doctor_EmployeeId",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Doctor_DoctorId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctor_Clinics_ClinicId",
                table: "Doctor");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctor_Positions_PositionId",
                table: "Doctor");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctor_Specialists_SpecialistId",
                table: "Doctor");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Doctor_EmployeeId",
                table: "Services");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Doctor",
                table: "Doctor");

            migrationBuilder.RenameTable(
                name: "Doctor",
                newName: "Doctors");

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
                table: "Blogs",
                newName: "DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_Blogs_EmployeeId",
                table: "Blogs",
                newName: "IX_Blogs_DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_Doctor_SpecialistId",
                table: "Doctors",
                newName: "IX_Doctors_SpecialistId");

            migrationBuilder.RenameIndex(
                name: "IX_Doctor_PositionId",
                table: "Doctors",
                newName: "IX_Doctors_PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_Doctor_ClinicId",
                table: "Doctors",
                newName: "IX_Doctors_ClinicId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Doctors",
                table: "Doctors",
                column: "Id");

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
                name: "FK_Doctors_Clinics_ClinicId",
                table: "Doctors",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Positions_PositionId",
                table: "Doctors",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Specialists_SpecialistId",
                table: "Doctors",
                column: "SpecialistId",
                principalTable: "Specialists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Doctors_DoctorId",
                table: "Services",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Doctors_DoctorId",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Doctors_DoctorId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Clinics_ClinicId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Positions_PositionId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Specialists_SpecialistId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Doctors_DoctorId",
                table: "Services");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Doctors",
                table: "Doctors");

            migrationBuilder.RenameTable(
                name: "Doctors",
                newName: "Doctor");

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
                table: "Blogs",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Blogs_DoctorId",
                table: "Blogs",
                newName: "IX_Blogs_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Doctors_SpecialistId",
                table: "Doctor",
                newName: "IX_Doctor_SpecialistId");

            migrationBuilder.RenameIndex(
                name: "IX_Doctors_PositionId",
                table: "Doctor",
                newName: "IX_Doctor_PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_Doctors_ClinicId",
                table: "Doctor",
                newName: "IX_Doctor_ClinicId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Doctor",
                table: "Doctor",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Doctor_EmployeeId",
                table: "Blogs",
                column: "EmployeeId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Doctor_DoctorId",
                table: "Courses",
                column: "DoctorId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctor_Clinics_ClinicId",
                table: "Doctor",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctor_Positions_PositionId",
                table: "Doctor",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctor_Specialists_SpecialistId",
                table: "Doctor",
                column: "SpecialistId",
                principalTable: "Specialists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Doctor_EmployeeId",
                table: "Services",
                column: "EmployeeId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
