using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkFlowManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class relationWorkFlowRoleCharttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowRoleCharts_OrganizationChartId",
                table: "WorkFlowRoleCharts",
                column: "OrganizationChartId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowRoleCharts_WorkFlowRoleId",
                table: "WorkFlowRoleCharts",
                column: "WorkFlowRoleId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkFlowRoleCharts_OrganizationCharts_OrganizationChartId",
                table: "WorkFlowRoleCharts");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkFlowRoleCharts_WorkFlowRoles_WorkFlowRoleId",
                table: "WorkFlowRoleCharts");

            migrationBuilder.DropIndex(
                name: "IX_WorkFlowRoleCharts_OrganizationChartId",
                table: "WorkFlowRoleCharts");

            migrationBuilder.DropIndex(
                name: "IX_WorkFlowRoleCharts_WorkFlowRoleId",
                table: "WorkFlowRoleCharts");
        }
    }
}
