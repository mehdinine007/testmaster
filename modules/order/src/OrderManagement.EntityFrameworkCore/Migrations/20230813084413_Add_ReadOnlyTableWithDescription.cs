using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddReadOnlyTableWithDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title_En",
                table: "ProductAndCategoryType_ReadOnly",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ProductAndCategoryType_ReadOnly",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title_En",
                table: "OrderStatusTypeReadOnly",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "OrderStatusTypeReadOnly",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "OrderDeliveryStatus",
                table: "OrderStatusInquiry",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Title_En",
                table: "OrderRejectionTypeReadOnly",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "OrderRejectionTypeReadOnly",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title_En",
                table: "OrderDeliveryStatusTypeReadOnly",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "OrderDeliveryStatusTypeReadOnly",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "OrderDeliveryStatusTypeReadOnly",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "OrderDeliveryStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Title_En" },
                values: new object[] { "لورم اپیسوم", "OrderRegistered" });

            migrationBuilder.UpdateData(
                table: "OrderDeliveryStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Title_En" },
                values: new object[] { "لورم اپیسوم", "Prioritization" });

            migrationBuilder.UpdateData(
                table: "OrderDeliveryStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Title_En" },
                values: new object[] { "لورم اپیسوم", "ProductDetermination" });

            migrationBuilder.UpdateData(
                table: "OrderDeliveryStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Title_En" },
                values: new object[] { "لورم اپیسوم", "SendingToManufaturer" });

            migrationBuilder.UpdateData(
                table: "OrderDeliveryStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "Title_En" },
                values: new object[] { "لورم اپیسوم", "ReceivingContractRowId" });

            migrationBuilder.UpdateData(
                table: "OrderDeliveryStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Description", "Title_En" },
                values: new object[] { "لورم اپیسوم", "ReceivingAmountCompleted" });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "OrderDeliveryStatusTypeReadOnly");

            migrationBuilder.AlterColumn<string>(
                name: "Title_En",
                table: "ProductAndCategoryType_ReadOnly",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ProductAndCategoryType_ReadOnly",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Title_En",
                table: "OrderStatusTypeReadOnly",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "OrderStatusTypeReadOnly",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<int>(
                name: "OrderDeliveryStatus",
                table: "OrderStatusInquiry",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title_En",
                table: "OrderRejectionTypeReadOnly",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "OrderRejectionTypeReadOnly",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Title_En",
                table: "OrderDeliveryStatusTypeReadOnly",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "OrderDeliveryStatusTypeReadOnly",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.UpdateData(
                table: "OrderDeliveryStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 1,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrderDeliveryStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrderDeliveryStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 3,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrderDeliveryStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 4,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrderDeliveryStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 5,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrderDeliveryStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 6,
                column: "Title_En",
                value: null);

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
