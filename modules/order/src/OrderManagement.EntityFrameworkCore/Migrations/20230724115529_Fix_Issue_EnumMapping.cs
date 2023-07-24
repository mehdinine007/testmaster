using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class FixIssueEnumMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "عدم تطابق کدملی و شماره موبایل", "PhoneNumberAndNationalCodeConflict" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "نداشتن گواهی نامه معتبر", "DoesntHadQualifiedDrivingLicense" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "دارای پلاک فعال", "ActivePlaqueDetected" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "ثبت سفارش در سامانه خودروهای وارداتی", "OrderRegisteredInInternalVehicleSite" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "لیست خرید خودروساز (سایپا)", "SaipaVehicleManufactureList" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "لیست خرید خودروساز (ایران خودرو)", "IkcoVehicleManufactureList" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "لیست خرید خودروساز (کرمان موتور)", "KermanMotorVehicleManufactureList" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "لیست خرید خودروساز (صنایع خودرو سازی ایلیا)", "IliaAutoVehicleManufactureList" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "لیست خرید خودروساز (فردا موتورز)", "FardaMotorsVehicleManufactureList" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "لیست خرید خودروساز (آرین پارس)", "ArianParsVehicleManufactureList" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "لیست خرید خودروساز (مکث موتور)", "MaxMotorVehicleManufactureList" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "لیست خرید خودروساز (بهمن موتور)", "BahmanMotorVehicleManufactureList" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "لیست خرید خودروساز (مدیران خودرو)", "MvmVehicleManufactureList" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "عدم احراز در طرح جوانی توسط ثبت احول", "YoungPlan" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "عدم احراض خودرو فرسوده", "OldPlan" });

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "ثبت سفارش اولیه با موفقیت انجام شد", "RecentlyAdded" });

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "انصراف داده شده", "Canceled" });

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "انتخاب نشده اید", "loser" });

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "برنده شده اید", "Winner" });

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "انصراف کلی از اولیت بندی", "FullCancel" });

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "پرداخت با موفقیت انجام شد", "PaymentSucceeded" });

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "پرداخت ناموفق", "PaymentNotVerified" });

            migrationBuilder.UpdateData(
                table: "ProductAndCategoryType_ReadOnly",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "محصول", "Product" });

            migrationBuilder.UpdateData(
                table: "ProductAndCategoryType_ReadOnly",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "دسته بندی", "Category" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "PhoneNumberAndNationalCodeConflict", null });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "DoesntHadQualifiedDrivingLicense", null });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "ActivePlaqueDetected", null });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "OrderRegisteredInInternalVehicleSite", null });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "SaipaVehicleManufactureList", null });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "IkcoVehicleManufactureList", null });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "KermanMotorVehicleManufactureList", null });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "IliaAutoVehicleManufactureList", null });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "FardaMotorsVehicleManufactureList", null });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "ArianParsVehicleManufactureList", null });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "MaxMotorVehicleManufactureList", null });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "BahmanMotorVehicleManufactureList", null });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "MvmVehicleManufactureList", null });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "YoungPlan", null });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "OldPlan", null });

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "RecentlyAdded", null });

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "Canceled", null });

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "loser", null });

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "Winner", null });

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "FullCancel", null });

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "PaymentSucceeded", null });

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "PaymentNotVerified", null });

            migrationBuilder.UpdateData(
                table: "ProductAndCategoryType_ReadOnly",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "Product", null });

            migrationBuilder.UpdateData(
                table: "ProductAndCategoryType_ReadOnly",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "Category", null });
        }
    }
}
