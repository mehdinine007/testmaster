using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class removeCircularSaleCode_SaleStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CircularSaleCode",
                table: "SaleDetail");

            migrationBuilder.RenameColumn(
                name: "SaleStatus",
                table: "SaleSchema",
                newName: "Code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Code",
                table: "SaleSchema",
                newName: "SaleStatus");

            migrationBuilder.AddColumn<int>(
                name: "CircularSaleCode",
                table: "SaleDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
