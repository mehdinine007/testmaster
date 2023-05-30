using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAmountRange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payment_FilterParam1_FilterParam2_FilterParam3_FilterParam4",
                schema: "dbo",
                table: "Payment");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                schema: "dbo",
                table: "Payment",
                type: "decimal(18,0)",
                precision: 18,
                scale: 0,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,0)",
                oldPrecision: 10);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_PaymentStatusId_FilterParam1_FilterParam2_FilterParam3_FilterParam4",
                schema: "dbo",
                table: "Payment",
                columns: new[] { "PaymentStatusId", "FilterParam1", "FilterParam2", "FilterParam3", "FilterParam4" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payment_PaymentStatusId_FilterParam1_FilterParam2_FilterParam3_FilterParam4",
                schema: "dbo",
                table: "Payment");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                schema: "dbo",
                table: "Payment",
                type: "decimal(10,0)",
                precision: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,0)",
                oldPrecision: 18,
                oldScale: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_FilterParam1_FilterParam2_FilterParam3_FilterParam4",
                schema: "dbo",
                table: "Payment",
                columns: new[] { "FilterParam1", "FilterParam2", "FilterParam3", "FilterParam4" });
        }
    }
}
