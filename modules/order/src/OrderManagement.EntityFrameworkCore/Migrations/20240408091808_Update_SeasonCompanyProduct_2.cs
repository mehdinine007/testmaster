using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class Update_SeasonCompanyProduct_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeasonCompanyProduct_ESaleType_EsaleTypeId",
                table: "SeasonCompanyProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_SeasonCompanyProduct_ProductAndCategory_CompanyId",
                table: "SeasonCompanyProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_SeasonCompanyProduct_ProductAndCategory_ProductId",
                table: "SeasonCompanyProduct");

            migrationBuilder.DropIndex(
                name: "IX_SeasonCompanyProduct_EsaleTypeId",
                table: "SeasonCompanyProduct");

            migrationBuilder.DropIndex(
                name: "IX_SeasonCompanyProduct_ProductId",
                table: "SeasonCompanyProduct");

            migrationBuilder.DropColumn(
                name: "CarTipId",
                table: "SeasonCompanyProduct");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "SeasonCompanyProduct");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "SeasonCompanyProduct");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "SeasonCompanyProduct");

            migrationBuilder.DropColumn(
                name: "EsaleTypeId",
                table: "SeasonCompanyProduct");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "SeasonCompanyProduct");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "SeasonCompanyProduct",
                newName: "SaleDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_SeasonCompanyProduct_CompanyId",
                table: "SeasonCompanyProduct",
                newName: "IX_SeasonCompanyProduct_SaleDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_SeasonCompanyProduct_SaleDetail_SaleDetailId",
                table: "SeasonCompanyProduct",
                column: "SaleDetailId",
                principalTable: "SaleDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeasonCompanyProduct_SaleDetail_SaleDetailId",
                table: "SeasonCompanyProduct");

            migrationBuilder.RenameColumn(
                name: "SaleDetailId",
                table: "SeasonCompanyProduct",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_SeasonCompanyProduct_SaleDetailId",
                table: "SeasonCompanyProduct",
                newName: "IX_SeasonCompanyProduct_CompanyId");

            migrationBuilder.AddColumn<int>(
                name: "CarTipId",
                table: "SeasonCompanyProduct",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "SeasonCompanyProduct",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "SeasonCompanyProduct",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "SeasonCompanyProduct",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EsaleTypeId",
                table: "SeasonCompanyProduct",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "SeasonCompanyProduct",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeasonCompanyProduct_EsaleTypeId",
                table: "SeasonCompanyProduct",
                column: "EsaleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonCompanyProduct_ProductId",
                table: "SeasonCompanyProduct",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_SeasonCompanyProduct_ESaleType_EsaleTypeId",
                table: "SeasonCompanyProduct",
                column: "EsaleTypeId",
                principalTable: "ESaleType",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SeasonCompanyProduct_ProductAndCategory_CompanyId",
                table: "SeasonCompanyProduct",
                column: "CompanyId",
                principalTable: "ProductAndCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeasonCompanyProduct_ProductAndCategory_ProductId",
                table: "SeasonCompanyProduct",
                column: "ProductId",
                principalTable: "ProductAndCategory",
                principalColumn: "Id");
        }
    }
}
