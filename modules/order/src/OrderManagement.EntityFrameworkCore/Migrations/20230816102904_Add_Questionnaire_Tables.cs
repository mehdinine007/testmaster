using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OrderManagement.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddQuestionnaireTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Questionnaire",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
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
                    table.PrimaryKey("PK_Questionnaire", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionTypeReadOnly",
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
                    table.PrimaryKey("PK_QuestionTypeReadOnly", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    QuestionType = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_Question", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Question_Questionnaire_QuestionnaireId",
                        column: x => x.QuestionnaireId,
                        principalTable: "Questionnaire",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionAnswer",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Hint = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    CustomValue = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    QuestionnaireId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_QuestionAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionAnswer_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionAnswer_Questionnaire_QuestionnaireId",
                        column: x => x.QuestionnaireId,
                        principalTable: "Questionnaire",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubmittedAnswer",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    QuestionAnswerId = table.Column<long>(type: "bigint", nullable: true),
                    CustomAnswerValue = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_SubmittedAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubmittedAnswer_QuestionAnswer_QuestionAnswerId",
                        column: x => x.QuestionAnswerId,
                        principalTable: "QuestionAnswer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubmittedAnswer_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.InsertData(
                table: "QuestionTypeReadOnly",
                columns: new[] { "Id", "Code", "Title", "Title_En" },
                values: new object[,]
                {
                    { 1, 1, "تشریحی", "Descriptional" },
                    { 2, 2, "گزینه ای", "Optional" },
                    { 3, 3, "بازه", "Range" },
                    { 4, 4, "چند گزینه ای", "MultiSelectOptional" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Question_QuestionnaireId",
                table: "Question",
                column: "QuestionnaireId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswer_QuestionId",
                table: "QuestionAnswer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswer_QuestionnaireId",
                table: "QuestionAnswer",
                column: "QuestionnaireId");

            migrationBuilder.CreateIndex(
                name: "IX_SubmittedAnswer_QuestionAnswerId",
                table: "SubmittedAnswer",
                column: "QuestionAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_SubmittedAnswer_QuestionId",
                table: "SubmittedAnswer",
                column: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionTypeReadOnly");

            migrationBuilder.DropTable(
                name: "SubmittedAnswer");

            migrationBuilder.DropTable(
                name: "QuestionAnswer");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "Questionnaire");

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
