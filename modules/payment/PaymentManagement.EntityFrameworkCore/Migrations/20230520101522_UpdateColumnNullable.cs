using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColumnNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "PspAccount",
                newName: "PspAccount",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Psp",
                newName: "Psp",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "PaymentStatus",
                newName: "PaymentStatus",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "PaymentLog",
                newName: "PaymentLog",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Payment",
                newName: "Payment",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Customer",
                newName: "Customer",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Account",
                newName: "Account",
                newSchema: "dbo");

            migrationBuilder.AlterColumn<int>(
                name: "FilterParam",
                schema: "dbo",
                table: "Payment",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "PspAccount",
                schema: "dbo",
                newName: "PspAccount");

            migrationBuilder.RenameTable(
                name: "Psp",
                schema: "dbo",
                newName: "Psp");

            migrationBuilder.RenameTable(
                name: "PaymentStatus",
                schema: "dbo",
                newName: "PaymentStatus");

            migrationBuilder.RenameTable(
                name: "PaymentLog",
                schema: "dbo",
                newName: "PaymentLog");

            migrationBuilder.RenameTable(
                name: "Payment",
                schema: "dbo",
                newName: "Payment");

            migrationBuilder.RenameTable(
                name: "Customer",
                schema: "dbo",
                newName: "Customer");

            migrationBuilder.RenameTable(
                name: "Account",
                schema: "dbo",
                newName: "Account");

            migrationBuilder.AlterColumn<string>(
                name: "FilterParam",
                table: "Payment",
                type: "VARCHAR(50)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
