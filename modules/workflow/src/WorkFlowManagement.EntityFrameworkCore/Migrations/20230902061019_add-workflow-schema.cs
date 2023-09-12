using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkFlowManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class addworkflowschema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkFlowRoleCharts_OrganizationCharts_OrganizationChartId",
                table: "WorkFlowRoleCharts");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkFlowRoleCharts_WorkFlowRoles_WorkFlowRoleId",
                table: "WorkFlowRoleCharts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkFlowRoles",
                table: "WorkFlowRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkFlowRoleCharts",
                table: "WorkFlowRoleCharts");

            migrationBuilder.EnsureSchema(
                name: "Flow");

            migrationBuilder.RenameTable(
                name: "WorkFlowRoles",
                newName: "Roles",
                newSchema: "Flow");

            migrationBuilder.RenameTable(
                name: "WorkFlowRoleCharts",
                newName: "RoleCharts");

            migrationBuilder.RenameIndex(
                name: "IX_WorkFlowRoleCharts_WorkFlowRoleId",
                table: "RoleCharts",
                newName: "IX_RoleCharts_WorkFlowRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkFlowRoleCharts_OrganizationChartId",
                table: "RoleCharts",
                newName: "IX_RoleCharts_OrganizationChartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                schema: "Flow",
                table: "Roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleCharts",
                table: "RoleCharts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleCharts_OrganizationCharts_OrganizationChartId",
                table: "RoleCharts",
                column: "OrganizationChartId",
                principalTable: "OrganizationCharts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleCharts_Roles_WorkFlowRoleId",
                table: "RoleCharts",
                column: "WorkFlowRoleId",
                principalSchema: "Flow",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleCharts_OrganizationCharts_OrganizationChartId",
                table: "RoleCharts");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleCharts_Roles_WorkFlowRoleId",
                table: "RoleCharts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                schema: "Flow",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleCharts",
                table: "RoleCharts");

            migrationBuilder.RenameTable(
                name: "Roles",
                schema: "Flow",
                newName: "WorkFlowRoles");

            migrationBuilder.RenameTable(
                name: "RoleCharts",
                newName: "WorkFlowRoleCharts");

            migrationBuilder.RenameIndex(
                name: "IX_RoleCharts_WorkFlowRoleId",
                table: "WorkFlowRoleCharts",
                newName: "IX_WorkFlowRoleCharts_WorkFlowRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleCharts_OrganizationChartId",
                table: "WorkFlowRoleCharts",
                newName: "IX_WorkFlowRoleCharts_OrganizationChartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkFlowRoles",
                table: "WorkFlowRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkFlowRoleCharts",
                table: "WorkFlowRoleCharts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkFlowRoleCharts_OrganizationCharts_OrganizationChartId",
                table: "WorkFlowRoleCharts",
                column: "OrganizationChartId",
                principalTable: "OrganizationCharts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkFlowRoleCharts_WorkFlowRoles_WorkFlowRoleId",
                table: "WorkFlowRoleCharts",
                column: "WorkFlowRoleId",
                principalTable: "WorkFlowRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
