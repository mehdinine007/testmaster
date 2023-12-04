using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReportManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class addroles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Roles",
                table: "Widgets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Roles",
                table: "Dashboards",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Roles",
                table: "Widgets");

            migrationBuilder.DropColumn(
                name: "Roles",
                table: "Dashboards");
        }
    }
}
