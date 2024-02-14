using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations.CompanyManagementDb
{
    /// <inheritdoc />
    public partial class companyshamsidate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TranDay",
                table: "CompanyPaypaidPrices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TranMonth",
                table: "CompanyPaypaidPrices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TranYear",
                table: "CompanyPaypaidPrices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "ClientsOrderDetailByCompany",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryDay",
                table: "ClientsOrderDetailByCompany",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryMonth",
                table: "ClientsOrderDetailByCompany",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryYear",
                table: "ClientsOrderDetailByCompany",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FactorDay",
                table: "ClientsOrderDetailByCompany",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FactorMonth",
                table: "ClientsOrderDetailByCompany",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FactorYear",
                table: "ClientsOrderDetailByCompany",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IntroductionDay",
                table: "ClientsOrderDetailByCompany",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IntroductionMonth",
                table: "ClientsOrderDetailByCompany",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IntroductionYear",
                table: "ClientsOrderDetailByCompany",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ClientsOrderDetailByCompany_CompanyId",
                table: "ClientsOrderDetailByCompany",
                column: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClientsOrderDetailByCompany_CompanyId",
                table: "ClientsOrderDetailByCompany");

            migrationBuilder.DropColumn(
                name: "TranDay",
                table: "CompanyPaypaidPrices");

            migrationBuilder.DropColumn(
                name: "TranMonth",
                table: "CompanyPaypaidPrices");

            migrationBuilder.DropColumn(
                name: "TranYear",
                table: "CompanyPaypaidPrices");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "ClientsOrderDetailByCompany");

            migrationBuilder.DropColumn(
                name: "DeliveryDay",
                table: "ClientsOrderDetailByCompany");

            migrationBuilder.DropColumn(
                name: "DeliveryMonth",
                table: "ClientsOrderDetailByCompany");

            migrationBuilder.DropColumn(
                name: "DeliveryYear",
                table: "ClientsOrderDetailByCompany");

            migrationBuilder.DropColumn(
                name: "FactorDay",
                table: "ClientsOrderDetailByCompany");

            migrationBuilder.DropColumn(
                name: "FactorMonth",
                table: "ClientsOrderDetailByCompany");

            migrationBuilder.DropColumn(
                name: "FactorYear",
                table: "ClientsOrderDetailByCompany");

            migrationBuilder.DropColumn(
                name: "IntroductionDay",
                table: "ClientsOrderDetailByCompany");

            migrationBuilder.DropColumn(
                name: "IntroductionMonth",
                table: "ClientsOrderDetailByCompany");

            migrationBuilder.DropColumn(
                name: "IntroductionYear",
                table: "ClientsOrderDetailByCompany");
        }
    }
}
