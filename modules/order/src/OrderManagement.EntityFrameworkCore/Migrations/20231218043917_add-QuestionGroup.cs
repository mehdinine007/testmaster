using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class addQuestionGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Priority",
                table: "Priority");

            migrationBuilder.RenameTable(
                name: "Priority",
                newName: "PriorityList");

            migrationBuilder.AlterColumn<string>(
                name: "TrackingCode",
                table: "CustomerOrder",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NationalCode",
                table: "PriorityList",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriorityList",
                table: "PriorityList",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "QuestionGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    QuestionnaireId = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionGroup_Questionnaire_QuestionnaireId",
                        column: x => x.QuestionnaireId,
                        principalTable: "Questionnaire",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "GenderTypeReadOnly",
                keyColumn: "Id",
                keyValue: 1,
                column: "Title_En",
                value: "Male");

            migrationBuilder.UpdateData(
                table: "GenderTypeReadOnly",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title_En",
                value: "Female");

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
                value: "CancelledBySystem");

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 7,
                column: "Title_En",
                value: "PaymentSucceeded");

            migrationBuilder.UpdateData(
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 8,
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
                value: "AuthorizedOnly");

            migrationBuilder.UpdateData(
                table: "QuestionnaireTypeReadOnly",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title_En",
                value: "AnonymousAllowed");

            migrationBuilder.UpdateData(
                table: "SaleProcessTypeReadOnly",
                keyColumn: "Id",
                keyValue: 1,
                column: "Title_En",
                value: "RegularSale");

            migrationBuilder.UpdateData(
                table: "SaleProcessTypeReadOnly",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title_En",
                value: "DirectSale");

            migrationBuilder.UpdateData(
                table: "SaleProcessTypeReadOnly",
                keyColumn: "Id",
                keyValue: 3,
                column: "Title_En",
                value: "SaleWithTrackingCode");

            migrationBuilder.InsertData(
                table: "SaleProcessTypeReadOnly",
                columns: new[] { "Id", "Code", "Title", "Title_En" },
                values: new object[] { 4, 3, "فروش آزاد", "FreeSale" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrder_TrackingCode",
                table: "CustomerOrder",
                column: "TrackingCode",
                unique: true,
                filter: "IsDeleted = 0 ");

            migrationBuilder.CreateIndex(
                name: "IX_PriorityList_NationalCode",
                table: "PriorityList",
                column: "NationalCode");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionGroup_QuestionnaireId",
                table: "QuestionGroup",
                column: "QuestionnaireId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionGroup");

            migrationBuilder.DropIndex(
                name: "IX_CustomerOrder_TrackingCode",
                table: "CustomerOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PriorityList",
                table: "PriorityList");

            migrationBuilder.DropIndex(
                name: "IX_PriorityList_NationalCode",
                table: "PriorityList");

            migrationBuilder.DeleteData(
                table: "SaleProcessTypeReadOnly",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.RenameTable(
                name: "PriorityList",
                newName: "Priority");

            migrationBuilder.AlterColumn<string>(
                name: "TrackingCode",
                table: "CustomerOrder",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NationalCode",
                table: "Priority",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Priority",
                table: "Priority",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "GenderTypeReadOnly",
                keyColumn: "Id",
                keyValue: 1,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "GenderTypeReadOnly",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title_En",
                value: null);

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
                table: "OrderStatusTypeReadOnly",
                keyColumn: "Id",
                keyValue: 8,
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

            migrationBuilder.UpdateData(
                table: "SaleProcessTypeReadOnly",
                keyColumn: "Id",
                keyValue: 1,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "SaleProcessTypeReadOnly",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title_En",
                value: null);

            migrationBuilder.UpdateData(
                table: "SaleProcessTypeReadOnly",
                keyColumn: "Id",
                keyValue: 3,
                column: "Title_En",
                value: null);
        }
    }
}
