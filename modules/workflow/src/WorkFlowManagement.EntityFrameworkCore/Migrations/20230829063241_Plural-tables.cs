using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkFlowManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class Pluraltables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationChart_OrganizationChart_ParentId",
                table: "OrganizationChart");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationPosition_OrganizationChart_OrganizationChartId",
                table: "OrganizationPosition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationPosition",
                table: "OrganizationPosition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationChart",
                table: "OrganizationChart");

            migrationBuilder.RenameTable(
                name: "OrganizationPosition",
                newName: "OrganizationPositions");

            migrationBuilder.RenameTable(
                name: "OrganizationChart",
                newName: "OrganizationCharts");

            migrationBuilder.RenameIndex(
                name: "IX_OrganizationPosition_OrganizationChartId",
                table: "OrganizationPositions",
                newName: "IX_OrganizationPositions_OrganizationChartId");

            migrationBuilder.RenameIndex(
                name: "IX_OrganizationChart_ParentId",
                table: "OrganizationCharts",
                newName: "IX_OrganizationCharts_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationPositions",
                table: "OrganizationPositions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationCharts",
                table: "OrganizationCharts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationCharts_OrganizationCharts_ParentId",
                table: "OrganizationCharts",
                column: "ParentId",
                principalTable: "OrganizationCharts",
                principalColumn: "Id");

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
                name: "FK_OrganizationCharts_OrganizationCharts_ParentId",
                table: "OrganizationCharts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationPositions_OrganizationCharts_OrganizationChartId",
                table: "OrganizationPositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationPositions",
                table: "OrganizationPositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationCharts",
                table: "OrganizationCharts");

            migrationBuilder.RenameTable(
                name: "OrganizationPositions",
                newName: "OrganizationPosition");

            migrationBuilder.RenameTable(
                name: "OrganizationCharts",
                newName: "OrganizationChart");

            migrationBuilder.RenameIndex(
                name: "IX_OrganizationPositions_OrganizationChartId",
                table: "OrganizationPosition",
                newName: "IX_OrganizationPosition_OrganizationChartId");

            migrationBuilder.RenameIndex(
                name: "IX_OrganizationCharts_ParentId",
                table: "OrganizationChart",
                newName: "IX_OrganizationChart_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationPosition",
                table: "OrganizationPosition",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationChart",
                table: "OrganizationChart",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationChart_OrganizationChart_ParentId",
                table: "OrganizationChart",
                column: "ParentId",
                principalTable: "OrganizationChart",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationPosition_OrganizationChart_OrganizationChartId",
                table: "OrganizationPosition",
                column: "OrganizationChartId",
                principalTable: "OrganizationChart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
