using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OrderManagement.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUnAuthorizedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Age",
                table: "UnAuthorizedUser",
                type: "Date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "EducationLevel",
                table: "UnAuthorizedUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "UnAuthorizedUser",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Occupation",
                table: "UnAuthorizedUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GenderTypeReadOnly",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleEn = table.Column<string>(name: "Title_En", type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenderTypeReadOnly", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "GenderTypeReadOnly",
                columns: new[] { "Id", "Code", "Title", "Title_En" },
                values: new object[,]
                {
                    { 1, 0, "مرد", "Male" },
                    { 2, 1, "زن", "Female" }
                });

            migrationBuilder.UpdateData(
                table: "OrderDeliveryStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 1,
                column: "Title_En",
                value: "OrderRegistered");

            migrationBuilder.UpdateData(
                table: "OrderDeliveryStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title_En",
                value: "Prioritization");

            migrationBuilder.UpdateData(
                table: "OrderDeliveryStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 3,
                column: "Title_En",
                value: "ProductDetermination");

            migrationBuilder.UpdateData(
                table: "OrderDeliveryStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 4,
                column: "Title_En",
                value: "SendingToManufaturer");

            migrationBuilder.UpdateData(
                table: "OrderDeliveryStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 5,
                column: "Title_En",
                value: "ReceivingContractRowId");

            migrationBuilder.UpdateData(
                table: "OrderDeliveryStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 6,
                column: "Title_En",
                value: "ReceivingAmountCompleted");

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

            migrationBuilder.UpdateData(
                table: "QuestionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 1,
                column: "Title_En",
                value: "Descriptional");

            migrationBuilder.UpdateData(
                table: "QuestionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title_En",
                value: "Optional");

            migrationBuilder.UpdateData(
                table: "QuestionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 3,
                column: "Title_En",
                value: "Range");

            migrationBuilder.UpdateData(
                table: "QuestionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 4,
                column: "Title_En",
                value: "MultiSelectOptional");

            migrationBuilder.UpdateData(
                table: "QuestionnaireTypeReadOnly",
                keyColumn: "Id",
                keyValue: 1,
                column: "Title_En",
                value: "AnonymousAllowed");

            migrationBuilder.UpdateData(
                table: "QuestionnaireTypeReadOnly",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title_En",
                value: "AuthorizedOnly");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenderTypeReadOnly");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "UnAuthorizedUser");

            migrationBuilder.DropColumn(
                name: "EducationLevel",
                table: "UnAuthorizedUser");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "UnAuthorizedUser");

            migrationBuilder.DropColumn(
                name: "Occupation",
                table: "UnAuthorizedUser");

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

            migrationBuilder.UpdateData(
                table: "QuestionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 1,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "QuestionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "QuestionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 3,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "QuestionTypeReadOnly",
                keyColumn: "Id",
                keyValue: 4,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "QuestionnaireTypeReadOnly",
                keyColumn: "Id",
                keyValue: 1,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "QuestionnaireTypeReadOnly",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title_En",
                value: null);
        }
    }
}
