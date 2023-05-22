using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class NullableColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IkcoPassword",
                schema: "dbo",
                table: "PspAccount");

            migrationBuilder.DropColumn(
                name: "IkcoUserName",
                schema: "dbo",
                table: "PspAccount");

            migrationBuilder.DropColumn(
                name: "IranKishAcceptorId",
                schema: "dbo",
                table: "PspAccount");

            migrationBuilder.DropColumn(
                name: "IranKishPassPhrase",
                schema: "dbo",
                table: "PspAccount");

            migrationBuilder.DropColumn(
                name: "IranKishTerminalId",
                schema: "dbo",
                table: "PspAccount");

            migrationBuilder.DropColumn(
                name: "MellatTerminalId",
                schema: "dbo",
                table: "PspAccount");

            migrationBuilder.DropColumn(
                name: "MellatUserName",
                schema: "dbo",
                table: "PspAccount");

            migrationBuilder.DropColumn(
                name: "MellatUserPassword",
                schema: "dbo",
                table: "PspAccount");

            migrationBuilder.DropColumn(
                name: "ParsianPin",
                schema: "dbo",
                table: "PspAccount");

            migrationBuilder.DropColumn(
                name: "SadadMerchantId",
                schema: "dbo",
                table: "PspAccount");

            migrationBuilder.DropColumn(
                name: "SadadTerminalId",
                schema: "dbo",
                table: "PspAccount");

            migrationBuilder.DropColumn(
                name: "SadadTerminalKey",
                schema: "dbo",
                table: "PspAccount");

            migrationBuilder.DropColumn(
                name: "SamanMID",
                schema: "dbo",
                table: "PspAccount");

            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                schema: "dbo",
                table: "PspAccount",
                type: "VARCHAR(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)");

            migrationBuilder.AddColumn<string>(
                name: "JsonProps",
                schema: "dbo",
                table: "PspAccount",
                type: "VARCHAR(500)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RetryCount",
                schema: "dbo",
                table: "Payment",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JsonProps",
                schema: "dbo",
                table: "PspAccount");

            migrationBuilder.DropColumn(
                name: "RetryCount",
                schema: "dbo",
                table: "Payment");

            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                schema: "dbo",
                table: "PspAccount",
                type: "VARCHAR(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(200)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IkcoPassword",
                schema: "dbo",
                table: "PspAccount",
                type: "VARCHAR(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IkcoUserName",
                schema: "dbo",
                table: "PspAccount",
                type: "VARCHAR(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IranKishAcceptorId",
                schema: "dbo",
                table: "PspAccount",
                type: "VARCHAR(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IranKishPassPhrase",
                schema: "dbo",
                table: "PspAccount",
                type: "VARCHAR(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IranKishTerminalId",
                schema: "dbo",
                table: "PspAccount",
                type: "VARCHAR(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MellatTerminalId",
                schema: "dbo",
                table: "PspAccount",
                type: "VARCHAR(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MellatUserName",
                schema: "dbo",
                table: "PspAccount",
                type: "VARCHAR(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MellatUserPassword",
                schema: "dbo",
                table: "PspAccount",
                type: "VARCHAR(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ParsianPin",
                schema: "dbo",
                table: "PspAccount",
                type: "VARCHAR(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SadadMerchantId",
                schema: "dbo",
                table: "PspAccount",
                type: "VARCHAR(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SadadTerminalId",
                schema: "dbo",
                table: "PspAccount",
                type: "VARCHAR(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SadadTerminalKey",
                schema: "dbo",
                table: "PspAccount",
                type: "VARCHAR(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SamanMID",
                schema: "dbo",
                table: "PspAccount",
                type: "VARCHAR(50)",
                nullable: false,
                defaultValue: "");
        }
    }
}
