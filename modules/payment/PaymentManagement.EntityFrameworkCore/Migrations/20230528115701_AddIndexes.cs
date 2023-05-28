using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PaymentLog_PaymentId",
                schema: "dbo",
                table: "PaymentLog");

            migrationBuilder.DropIndex(
                name: "IX_Payment_PaymentStatusId",
                schema: "dbo",
                table: "Payment");

            migrationBuilder.CreateIndex(
                name: "IX_PspAccount_IsActive",
                schema: "dbo",
                table: "PspAccount",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Psp_IsActive",
                schema: "dbo",
                table: "Psp",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentLog_PaymentId_Message",
                schema: "dbo",
                table: "PaymentLog",
                columns: new[] { "PaymentId", "Message" });

            migrationBuilder.CreateIndex(
                name: "IX_Payment_FilterParam1_FilterParam2_FilterParam3_FilterParam4",
                schema: "dbo",
                table: "Payment",
                columns: new[] { "FilterParam1", "FilterParam2", "FilterParam3", "FilterParam4" });

            migrationBuilder.CreateIndex(
                name: "IX_Payment_PaymentStatusId_TransactionDate_RetryCount",
                schema: "dbo",
                table: "Payment",
                columns: new[] { "PaymentStatusId", "TransactionDate", "RetryCount" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PspAccount_IsActive",
                schema: "dbo",
                table: "PspAccount");

            migrationBuilder.DropIndex(
                name: "IX_Psp_IsActive",
                schema: "dbo",
                table: "Psp");

            migrationBuilder.DropIndex(
                name: "IX_PaymentLog_PaymentId_Message",
                schema: "dbo",
                table: "PaymentLog");

            migrationBuilder.DropIndex(
                name: "IX_Payment_FilterParam1_FilterParam2_FilterParam3_FilterParam4",
                schema: "dbo",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_PaymentStatusId_TransactionDate_RetryCount",
                schema: "dbo",
                table: "Payment");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentLog_PaymentId",
                schema: "dbo",
                table: "PaymentLog",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_PaymentStatusId",
                schema: "dbo",
                table: "Payment",
                column: "PaymentStatusId");
        }
    }
}
