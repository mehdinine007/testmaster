using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerPriority : Migration
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
                name: "CustomerPriority",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApproximatePriority = table.Column<int>(type: "int", nullable: false),
                    SaleId = table.Column<int>(type: "int", nullable: false),
                    ChosenPriorityByCustomer = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_CustomerPriority", x => x.Id);
                });

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
                name: "IX_CustomerPriority_Uid",
                table: "CustomerPriority",
                column: "Uid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerPriority");

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
        }
    }
}
