using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReportManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class addoutputtypewidget : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OutPutType",
                table: "Widgets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutPutType",
                table: "Widgets");
        }
    }
}
