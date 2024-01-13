using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddActiveAnnouncement : Migration
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

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Announcement",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Announcement");

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
