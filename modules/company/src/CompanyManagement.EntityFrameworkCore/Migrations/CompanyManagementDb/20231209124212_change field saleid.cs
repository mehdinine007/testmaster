using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations.CompanyManagementDb
{
    /// <inheritdoc />
    public partial class changefieldsaleid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaleId",
                table: "ClientsOrderDetailByCompany");

            migrationBuilder.AddColumn<string>(
                name: "CompanySaleId",
                table: "ClientsOrderDetailByCompany",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanySaleId",
                table: "ClientsOrderDetailByCompany");

            migrationBuilder.AddColumn<int>(
                name: "SaleId",
                table: "ClientsOrderDetailByCompany",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
