using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class addproductidsaledetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "SaleDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 1,
                column: "Title_En",
                value: "PhoneNumberAndNationalCodeConflict");

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title_En",
                value: "DoesntHadQualifiedDrivingLicense");

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 3,
                column: "Title_En",
                value: "ActivePlaqueDetected");

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 4,
                column: "Title_En",
                value: "OrderRegisteredInInternalVehicleSite");

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 5,
                column: "Title_En",
                value: "SaipaVehicleManufactureList");

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 6,
                column: "Title_En",
                value: "IkcoVehicleManufactureList");

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 7,
                column: "Title_En",
                value: "KermanMotorVehicleManufactureList");

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 8,
                column: "Title_En",
                value: "IliaAutoVehicleManufactureList");

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 9,
                column: "Title_En",
                value: "FardaMotorsVehicleManufactureList");

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 10,
                column: "Title_En",
                value: "ArianParsVehicleManufactureList");

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 11,
                column: "Title_En",
                value: "MaxMotorVehicleManufactureList");

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 12,
                column: "Title_En",
                value: "BahmanMotorVehicleManufactureList");

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 13,
                column: "Title_En",
                value: "MvmVehicleManufactureList");

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 14,
                column: "Title_En",
                value: "YoungPlan");

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 15,
                column: "Title_En",
                value: "OldPlan");

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 1,
                column: "Title_En",
                value: "RecentlyAdded");

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title_En",
                value: "Canceled");

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 3,
                column: "Title_En",
                value: "loser");

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 4,
                column: "Title_En",
                value: "Winner");

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 5,
                column: "Title_En",
                value: "FullCancel");

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 6,
                column: "Title_En",
                value: "PaymentSucceeded");

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 7,
                column: "Title_En",
                value: "PaymentNotVerified");

            migrationBuilder.UpdateData(
                table: "ProductAndCategoryType_ReadOnly",
                keyColumn: "Id",
                keyValue: 1,
                column: "Title_En",
                value: "Product");

            migrationBuilder.UpdateData(
                table: "ProductAndCategoryType_ReadOnly",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title_En",
                value: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetail_ProductId",
                table: "SaleDetail",
                column: "ProductId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SaleDetail_ProductAndCategory_ProductId",
                table: "SaleDetail",
                column: "ProductId",
                principalTable: "ProductAndCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleDetail_ProductAndCategory_ProductId",
                table: "SaleDetail");

            migrationBuilder.DropIndex(
                name: "IX_SaleDetail_ProductId",
                table: "SaleDetail");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "SaleDetail");

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 1,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 3,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 4,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 5,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 6,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 7,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 8,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 9,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 10,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 11,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 12,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 13,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 14,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 15,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 1,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 3,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 4,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 5,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 6,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 7,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "ProductAndCategoryType_ReadOnly",
                keyColumn: "Id",
                keyValue: 1,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "ProductAndCategoryType_ReadOnly",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title_En",
                value: null);
        }
    }
}
