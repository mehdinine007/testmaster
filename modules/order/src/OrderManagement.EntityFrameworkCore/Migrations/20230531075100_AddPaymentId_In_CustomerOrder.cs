using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentIdInCustomerOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "CustomerOrder",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 6,
                column: "OrderStatusCode",
                value: 70);

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 7,
                column: "OrderStatusCode",
                value: 80);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "CustomerOrder");

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 6,
                column: "OrderStatusCode",
                value: 60);

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 7,
                column: "OrderStatusCode",
                value: 70);
        }
    }
}
