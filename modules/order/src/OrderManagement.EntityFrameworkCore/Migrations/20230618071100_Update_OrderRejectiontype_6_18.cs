using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderRejectiontype618 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "OrderRejectionTypeReadOnly",
                columns: new[] { "Id", "OrderRejectionCode", "OrderRejectionTitle", "OrderRejectionTitleEn" },
                values: new object[] { 15, 15, "عدم احراض خودرو فرسوده", "OldPlan" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 15);
        }
    }
}
