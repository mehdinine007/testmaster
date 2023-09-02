using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkFlowManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class addworkflowschemaalltable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "RoleCharts",
                newName: "RoleCharts",
                newSchema: "Flow");

            migrationBuilder.RenameTable(
                name: "OrganizationPositions",
                newName: "OrganizationPositions",
                newSchema: "Flow");

            migrationBuilder.RenameTable(
                name: "OrganizationCharts",
                newName: "OrganizationCharts",
                newSchema: "Flow");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "RoleCharts",
                schema: "Flow",
                newName: "RoleCharts");

            migrationBuilder.RenameTable(
                name: "OrganizationPositions",
                schema: "Flow",
                newName: "OrganizationPositions");

            migrationBuilder.RenameTable(
                name: "OrganizationCharts",
                schema: "Flow",
                newName: "OrganizationCharts");
        }
    }
}
