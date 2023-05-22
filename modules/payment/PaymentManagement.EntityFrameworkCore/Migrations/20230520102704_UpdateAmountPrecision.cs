using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAmountPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                schema: "dbo",
                table: "Payment",
                type: "decimal(10,0)",
                precision: 10,
                scale: 0,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                schema: "dbo",
                table: "Payment",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,0)",
                oldPrecision: 10,
                oldScale: 0);
        }
    }
}
