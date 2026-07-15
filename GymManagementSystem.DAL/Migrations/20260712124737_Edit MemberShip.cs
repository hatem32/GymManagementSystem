using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagementSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EditMemberShip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberShips_Plans_PlanId",
                table: "MemberShips");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberShips_Plans_PlanId",
                table: "MemberShips",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberShips_Plans_PlanId",
                table: "MemberShips");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberShips_Plans_PlanId",
                table: "MemberShips",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
