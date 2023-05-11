using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OrderManagement.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddReadOnlyTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderRejectionTypeReadOnly",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderRejectionCode = table.Column<int>(type: "int", nullable: false),
                    OrderRejectionTitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderRejectionTitle = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderRejectionTypeReadOnly", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatusTypeReadOnly",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderStatusTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderStatusCode = table.Column<int>(type: "int", nullable: false),
                    OrderStatusTitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatusTypeReadOnly", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "OrderRejectionTypeReadOnly",
                columns: new[] { "Id", "OrderRejectionCode", "OrderRejectionTitle", "OrderRejectionTitleEn" },
                values: new object[,]
                {
                    { 1, 1, "عدم تطابق کدملی و شماره موبایل", "PhoneNumberAndNationalCodeConflict" },
                    { 2, 2, "نداشتن گواهی نامه معتبر", "DoesntHadQualifiedDrivingLicense" },
                    { 3, 3, "دارای پلاک فعال", "ActivePlaqueDetected" },
                    { 4, 4, "ثبت سفارش در سامانه خودروهای وارداتی", "OrderRegisteredInInternalVehicleSite" },
                    { 5, 5, "لیست خرید خودروساز (سایپا)", "SaipaVehicleManufactureList" },
                    { 6, 6, "لیست خرید خودروساز (ایران خودرو)", "IkcoVehicleManufactureList" },
                    { 7, 7, "لیست خرید خودروساز (کرمان موتور)", "KermanMotorVehicleManufactureList" },
                    { 8, 8, "لیست خرید خودروساز (صنایع خودرو سازی ایلیا)", "IliaAutoVehicleManufactureList" },
                    { 9, 9, "لیست خرید خودروساز (فردا موتورز)", "FardaMotorsVehicleManufactureList" },
                    { 10, 10, "لیست خرید خودروساز (آرین پارس)", "ArianParsVehicleManufactureList" },
                    { 11, 11, "لیست خرید خودروساز (مکث موتور)", "MaxMotorVehicleManufactureList" },
                    { 12, 12, "لیست خرید خودروساز (بهمن موتور)", "BahmanMotorVehicleManufactureList" },
                    { 13, 13, "لیست خرید خودروساز (مدیران خودرو)", "MvmVehicleManufactureList" },
                    { 14, 14, "عدم احراز در طرح جوانی توسط ثبت احول", "YoungPlan" }
                });

            migrationBuilder.InsertData(
                table: "OrderStatusTypeReadOnly",
                columns: new[] { "Id", "OrderStatusCode", "OrderStatusTitle", "OrderStatusTitleEn" },
                values: new object[,]
                {
                    { 1, 10, "ثبت سفارش اولیه با موفقیت انجام شد", "RecentlyAdded" },
                    { 2, 20, "انصراف داده شده", "Canceled" },
                    { 3, 30, "انتخاب نشده اید", "loser" },
                    { 4, 40, "برنده شده اید", "Winner" },
                    { 5, 50, "انصراف کلی از اولیت بندی", "FullCancel" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderRejectionTypeReadOnly");

            migrationBuilder.DropTable(
                name: "OrderStatusTypeReadOnly");
        }
    }
}
