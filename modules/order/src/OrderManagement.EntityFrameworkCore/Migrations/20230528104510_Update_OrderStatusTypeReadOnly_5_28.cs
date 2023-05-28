using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderStatusTypeReadOnly528 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AgencyId",
                table: "CustomerOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PspId",
                table: "CustomerOrder",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "OrderStatusTypeReadOnly",
                columns: new[] { "Id", "OrderStatusCode", "OrderStatusTitle", "OrderStatusTitleEn" },
                values: new object[] { 7, 70, "پرداخت ناموفق", "PaymentNotVerified" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DropColumn(
                name: "AgencyId",
                table: "CustomerOrder");

            migrationBuilder.DropColumn(
                name: "PspId",
                table: "CustomerOrder");
        }
    }
}
