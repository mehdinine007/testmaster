using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OrderManagement.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddProductAndCategoryTypeReadOnlytb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderStatusTitle",
                table: "OrderStatusTypeReadOnly");

            migrationBuilder.DropColumn(
                name: "OrderStatusTitleEn",
                table: "OrderStatusTypeReadOnly");

            migrationBuilder.DropColumn(
                name: "OrderRejectionTitle",
                table: "OrderRejectionTypeReadOnly");

            migrationBuilder.DropColumn(
                name: "OrderRejectionTitleEn",
                table: "OrderRejectionTypeReadOnly");

            migrationBuilder.RenameColumn(
                name: "OrderStatusCode",
                table: "OrderStatusTypeReadOnly",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "OrderRejectionCode",
                table: "OrderRejectionTypeReadOnly",
                newName: "Code");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "OrderStatusTypeReadOnly",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title_En",
                table: "OrderStatusTypeReadOnly",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "OrderRejectionTypeReadOnly",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title_En",
                table: "OrderRejectionTypeReadOnly",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ProductAndCategoryType_ReadOnly",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleEn = table.Column<string>(name: "Title_En", type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAndCategoryType_ReadOnly", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "PhoneNumberAndNationalCodeConflict", "عدم تطابق کدملی و شماره موبایل" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "DoesntHadQualifiedDrivingLicense", "نداشتن گواهی نامه معتبر" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "ActivePlaqueDetected", "دارای پلاک فعال" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "OrderRegisteredInInternalVehicleSite", "ثبت سفارش در سامانه خودروهای وارداتی" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "SaipaVehicleManufactureList", "لیست خرید خودروساز (سایپا)" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "IkcoVehicleManufactureList", "لیست خرید خودروساز (ایران خودرو)" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "KermanMotorVehicleManufactureList", "لیست خرید خودروساز (کرمان موتور)" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "IliaAutoVehicleManufactureList", "لیست خرید خودروساز (صنایع خودرو سازی ایلیا)" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "FardaMotorsVehicleManufactureList", "لیست خرید خودروساز (فردا موتورز)" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "ArianParsVehicleManufactureList", "لیست خرید خودروساز (آرین پارس)" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "MaxMotorVehicleManufactureList", "لیست خرید خودروساز (مکث موتور)" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "BahmanMotorVehicleManufactureList", "لیست خرید خودروساز (بهمن موتور)" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "MvmVehicleManufactureList", "لیست خرید خودروساز (مدیران خودرو)" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "YoungPlan", "عدم احراز در طرح جوانی توسط ثبت احول" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "OldPlan", "عدم احراض خودرو فرسوده" });

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "RecentlyAdded", "ثبت سفارش اولیه با موفقیت انجام شد" });

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "Canceled", "انصراف داده شده" });

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "loser", "انتخاب نشده اید" });

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "Winner", "برنده شده اید" });

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "FullCancel", "انصراف کلی از اولیت بندی" });

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "PaymentSucceeded", "پرداخت با موفقیت انجام شد" });

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Title", "Title_En" },
                values: new object[] { "PaymentNotVerified", "پرداخت ناموفق" });

            migrationBuilder.InsertData(
                table: "ProductAndCategoryType_ReadOnly",
                columns: new[] { "Id", "Code", "Title", "Title_En" },
                values: new object[,]
                {
                    { 1, 1, "Product", "محصول" },
                    { 2, 2, "Category", "دسته بندی" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductAndCategoryType_ReadOnly");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "OrderStatusTypeReadOnly");

            migrationBuilder.DropColumn(
                name: "Title_En",
                table: "OrderStatusTypeReadOnly");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "OrderRejectionTypeReadOnly");

            migrationBuilder.DropColumn(
                name: "Title_En",
                table: "OrderRejectionTypeReadOnly");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "OrderStatusTypeReadOnly",
                newName: "OrderStatusCode");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "OrderRejectionTypeReadOnly",
                newName: "OrderRejectionCode");

            migrationBuilder.AddColumn<string>(
                name: "OrderStatusTitle",
                table: "OrderStatusTypeReadOnly",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderStatusTitleEn",
                table: "OrderStatusTypeReadOnly",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderRejectionTitle",
                table: "OrderRejectionTypeReadOnly",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderRejectionTitleEn",
                table: "OrderRejectionTypeReadOnly",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "OrderRejectionTitle", "OrderRejectionTitleEn" },
                values: new object[] { "عدم تطابق کدملی و شماره موبایل", "PhoneNumberAndNationalCodeConflict" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "OrderRejectionTitle", "OrderRejectionTitleEn" },
                values: new object[] { "نداشتن گواهی نامه معتبر", "DoesntHadQualifiedDrivingLicense" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "OrderRejectionTitle", "OrderRejectionTitleEn" },
                values: new object[] { "دارای پلاک فعال", "ActivePlaqueDetected" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "OrderRejectionTitle", "OrderRejectionTitleEn" },
                values: new object[] { "ثبت سفارش در سامانه خودروهای وارداتی", "OrderRegisteredInInternalVehicleSite" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "OrderRejectionTitle", "OrderRejectionTitleEn" },
                values: new object[] { "لیست خرید خودروساز (سایپا)", "SaipaVehicleManufactureList" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "OrderRejectionTitle", "OrderRejectionTitleEn" },
                values: new object[] { "لیست خرید خودروساز (ایران خودرو)", "IkcoVehicleManufactureList" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "OrderRejectionTitle", "OrderRejectionTitleEn" },
                values: new object[] { "لیست خرید خودروساز (کرمان موتور)", "KermanMotorVehicleManufactureList" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "OrderRejectionTitle", "OrderRejectionTitleEn" },
                values: new object[] { "لیست خرید خودروساز (صنایع خودرو سازی ایلیا)", "IliaAutoVehicleManufactureList" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "OrderRejectionTitle", "OrderRejectionTitleEn" },
                values: new object[] { "لیست خرید خودروساز (فردا موتورز)", "FardaMotorsVehicleManufactureList" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "OrderRejectionTitle", "OrderRejectionTitleEn" },
                values: new object[] { "لیست خرید خودروساز (آرین پارس)", "ArianParsVehicleManufactureList" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "OrderRejectionTitle", "OrderRejectionTitleEn" },
                values: new object[] { "لیست خرید خودروساز (مکث موتور)", "MaxMotorVehicleManufactureList" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "OrderRejectionTitle", "OrderRejectionTitleEn" },
                values: new object[] { "لیست خرید خودروساز (بهمن موتور)", "BahmanMotorVehicleManufactureList" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "OrderRejectionTitle", "OrderRejectionTitleEn" },
                values: new object[] { "لیست خرید خودروساز (مدیران خودرو)", "MvmVehicleManufactureList" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "OrderRejectionTitle", "OrderRejectionTitleEn" },
                values: new object[] { "عدم احراز در طرح جوانی توسط ثبت احول", "YoungPlan" });

            migrationBuilder.UpdateData(
                table: "OrderRejectionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "OrderRejectionTitle", "OrderRejectionTitleEn" },
                values: new object[] { "عدم احراض خودرو فرسوده", "OldPlan" });

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "OrderStatusTitle", "OrderStatusTitleEn" },
                values: new object[] { "ثبت سفارش اولیه با موفقیت انجام شد", "RecentlyAdded" });

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "OrderStatusTitle", "OrderStatusTitleEn" },
                values: new object[] { "انصراف داده شده", "Canceled" });

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "OrderStatusTitle", "OrderStatusTitleEn" },
                values: new object[] { "انتخاب نشده اید", "loser" });

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "OrderStatusTitle", "OrderStatusTitleEn" },
                values: new object[] { "برنده شده اید", "Winner" });

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "OrderStatusTitle", "OrderStatusTitleEn" },
                values: new object[] { "انصراف کلی از اولیت بندی", "FullCancel" });

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "OrderStatusTitle", "OrderStatusTitleEn" },
                values: new object[] { "پرداخت با موفقیت انجام شد", "PaymentSucceeded" });

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "OrderStatusTitle", "OrderStatusTitleEn" },
                values: new object[] { "پرداخت ناموفق", "PaymentNotVerified" });
        }
    }
}
