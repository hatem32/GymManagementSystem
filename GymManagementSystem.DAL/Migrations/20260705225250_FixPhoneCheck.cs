using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagementSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixPhoneCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "PhoneCheck1",
                table: "Trainers");

            migrationBuilder.DropCheckConstraint(
                name: "PhoneCheck",
                table: "Members");

            migrationBuilder.AlterColumn<int>(
                name: "Address_BuildingNumber",
                table: "Trainers",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Address_BuildingNumber",
                table: "Members",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddCheckConstraint(
                name: "PhoneCheck1",
                table: "Trainers",
                sql: "Phone Like '010%' or Phone Like '011%' or Phone Like '012%' or Phone Like '015%' ");

            migrationBuilder.AddCheckConstraint(
                name: "PhoneCheck",
                table: "Members",
                sql: "Phone Like '010%' or Phone Like '011%' or Phone Like '012%' or Phone Like '015%' ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "PhoneCheck1",
                table: "Trainers");

            migrationBuilder.DropCheckConstraint(
                name: "PhoneCheck",
                table: "Members");

            migrationBuilder.AlterColumn<string>(
                name: "Address_BuildingNumber",
                table: "Trainers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Address_BuildingNumber",
                table: "Members",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddCheckConstraint(
                name: "PhoneCheck1",
                table: "Trainers",
                sql: "Phone Like '010%' or Phone Like '011' or Phone Like '012' or Phone Like '015%' ");

            migrationBuilder.AddCheckConstraint(
                name: "PhoneCheck",
                table: "Members",
                sql: "Phone Like '010%' or Phone Like '011' or Phone Like '012' or Phone Like '015%' ");
        }
    }
}
