using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class addpaymentcustomerorder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PaymentPrice",
                table: "CustomerOrder",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TransactionCommitDate",
                table: "CustomerOrder",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "CustomerOrder",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentPrice",
                table: "CustomerOrder");

            migrationBuilder.DropColumn(
                name: "TransactionCommitDate",
                table: "CustomerOrder");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "CustomerOrder");
        }
    }
}
