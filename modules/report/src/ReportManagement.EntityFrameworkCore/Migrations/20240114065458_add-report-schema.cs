using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReportManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class addreportschema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Rpt");

            migrationBuilder.RenameTable(
                name: "Widgets",
                newName: "Widgets",
                newSchema: "Rpt");

            migrationBuilder.RenameTable(
                name: "DashboardWidgets",
                newName: "DashboardWidgets",
                newSchema: "Rpt");

            migrationBuilder.RenameTable(
                name: "Dashboards",
                newName: "Dashboards",
                newSchema: "Rpt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Widgets",
                schema: "Rpt",
                newName: "Widgets");

            migrationBuilder.RenameTable(
                name: "DashboardWidgets",
                schema: "Rpt",
                newName: "DashboardWidgets");

            migrationBuilder.RenameTable(
                name: "Dashboards",
                schema: "Rpt",
                newName: "Dashboards");
        }
    }
}
