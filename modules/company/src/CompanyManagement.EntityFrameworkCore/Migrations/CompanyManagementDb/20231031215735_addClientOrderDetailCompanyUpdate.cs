using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations.CompanyManagementDb
{
    /// <inheritdoc />
    public partial class addClientOrderDetailCompanyUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarCode",
                table: "ClientsOrderDetailByCompany");

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "ClientsOrderDetailByCompany",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TranDate",
                table: "ClientsOrderDetailByCompany",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "ClientsOrderDetailByCompany");

            migrationBuilder.DropColumn(
                name: "TranDate",
                table: "ClientsOrderDetailByCompany");

            migrationBuilder.AddColumn<string>(
                name: "CarCode",
                table: "ClientsOrderDetailByCompany",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
