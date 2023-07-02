using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddrltnSaleDetailSaleSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SaleDetail_SaleId",
                table: "SaleDetail",
                column: "SaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleDetail_SaleSchema_SaleId",
                table: "SaleDetail",
                column: "SaleId",
                principalTable: "SaleSchema",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleDetail_SaleSchema_SaleId",
                table: "SaleDetail");

            migrationBuilder.DropIndex(
                name: "IX_SaleDetail_SaleId",
                table: "SaleDetail");
        }
    }
}
