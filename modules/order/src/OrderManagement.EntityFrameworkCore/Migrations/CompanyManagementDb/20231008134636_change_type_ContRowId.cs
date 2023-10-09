using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations.CompanyManagementDb
{
    /// <inheritdoc />
    public partial class changetypeContRowId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ContRowId",
                table: "ClientsOrderDetailByCompany",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ContRowId",
                table: "ClientsOrderDetailByCompany",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
