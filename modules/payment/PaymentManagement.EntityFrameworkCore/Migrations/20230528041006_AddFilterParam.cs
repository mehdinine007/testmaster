using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class AddFilterParam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilterParam",
                schema: "dbo",
                table: "Payment");

            migrationBuilder.AddColumn<int>(
                name: "FilterParam1",
                schema: "dbo",
                table: "Payment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FilterParam2",
                schema: "dbo",
                table: "Payment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FilterParam3",
                schema: "dbo",
                table: "Payment",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilterParam1",
                schema: "dbo",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "FilterParam2",
                schema: "dbo",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "FilterParam3",
                schema: "dbo",
                table: "Payment");

            migrationBuilder.AddColumn<int>(
                name: "FilterParam",
                schema: "dbo",
                table: "Payment",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
