using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkFlowManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class changerelationorganizationposition1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OrganizationPositions_OrganizationChartId",
                table: "OrganizationPositions",
                column: "OrganizationChartId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationPositions_OrganizationCharts_OrganizationChartId",
                table: "OrganizationPositions",
                column: "OrganizationChartId",
                principalTable: "OrganizationCharts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationPositions_OrganizationCharts_OrganizationChartId",
                table: "OrganizationPositions");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationPositions_OrganizationChartId",
                table: "OrganizationPositions");
        }
    }
}
