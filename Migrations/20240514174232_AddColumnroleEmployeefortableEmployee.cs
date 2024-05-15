using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediNet_BE.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnroleEmployeefortableEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleEmployee",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleEmployee",
                table: "Employees");
        }
    }
}
