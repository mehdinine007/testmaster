using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkFlowManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class removeWorkflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleCharts_Roles_WorkFlowRoleId",
                schema: "Flow",
                table: "RoleCharts");

            migrationBuilder.RenameColumn(
                name: "WorkFlowRoleId",
                schema: "Flow",
                table: "RoleCharts",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleCharts_WorkFlowRoleId",
                schema: "Flow",
                table: "RoleCharts",
                newName: "IX_RoleCharts_RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleCharts_Roles_RoleId",
                schema: "Flow",
                table: "RoleCharts",
                column: "RoleId",
                principalSchema: "Flow",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleCharts_Roles_RoleId",
                schema: "Flow",
                table: "RoleCharts");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                schema: "Flow",
                table: "RoleCharts",
                newName: "WorkFlowRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleCharts_RoleId",
                schema: "Flow",
                table: "RoleCharts",
                newName: "IX_RoleCharts_WorkFlowRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleCharts_Roles_WorkFlowRoleId",
                schema: "Flow",
                table: "RoleCharts",
                column: "WorkFlowRoleId",
                principalSchema: "Flow",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
