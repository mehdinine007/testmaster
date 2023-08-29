using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkFlowManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class addOrganizationType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganizationType",
                table: "OrganizationCharts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrganizationType",
                table: "OrganizationCharts");
        }
    }
}
