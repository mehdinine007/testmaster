using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentManagement.EntityFrameworkCore.Migrations
{
    public partial class UpdateDBSetName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Customers_CustomerId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentLogs_Payments_PaymentId",
                table: "PaymentLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentStatuss_PaymentStatusId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PspAccounts_PspAccountId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_PspAccounts_Accounts_AccountId",
                table: "PspAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_PspAccounts_Psps_PspId",
                table: "PspAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Psps",
                table: "Psps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PspAccounts",
                table: "PspAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentStatuss",
                table: "PaymentStatuss");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payments",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentLogs",
                table: "PaymentLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            migrationBuilder.RenameTable(
                name: "Psps",
                newName: "Psp");

            migrationBuilder.RenameTable(
                name: "PspAccounts",
                newName: "PspAccount");

            migrationBuilder.RenameTable(
                name: "PaymentStatuss",
                newName: "PaymentStatus");

            migrationBuilder.RenameTable(
                name: "Payments",
                newName: "Payment");

            migrationBuilder.RenameTable(
                name: "PaymentLogs",
                newName: "PaymentLog");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Customer");

            migrationBuilder.RenameTable(
                name: "Accounts",
                newName: "Account");

            migrationBuilder.RenameIndex(
                name: "IX_PspAccounts_PspId",
                table: "PspAccount",
                newName: "IX_PspAccount_PspId");

            migrationBuilder.RenameIndex(
                name: "IX_PspAccounts_AccountId",
                table: "PspAccount",
                newName: "IX_PspAccount_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_PspAccountId",
                table: "Payment",
                newName: "IX_Payment_PspAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_PaymentStatusId",
                table: "Payment",
                newName: "IX_Payment_PaymentStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentLogs_PaymentId",
                table: "PaymentLog",
                newName: "IX_PaymentLog_PaymentId");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_CustomerId",
                table: "Account",
                newName: "IX_Account_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Psp",
                table: "Psp",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PspAccount",
                table: "PspAccount",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentStatus",
                table: "PaymentStatus",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payment",
                table: "Payment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentLog",
                table: "PaymentLog",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customer",
                table: "Customer",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Account",
                table: "Account",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Customer_CustomerId",
                table: "Account",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_PaymentStatus_PaymentStatusId",
                table: "Payment",
                column: "PaymentStatusId",
                principalTable: "PaymentStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_PspAccount_PspAccountId",
                table: "Payment",
                column: "PspAccountId",
                principalTable: "PspAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentLog_Payment_PaymentId",
                table: "PaymentLog",
                column: "PaymentId",
                principalTable: "Payment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PspAccount_Account_AccountId",
                table: "PspAccount",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PspAccount_Psp_PspId",
                table: "PspAccount",
                column: "PspId",
                principalTable: "Psp",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Customer_CustomerId",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_PaymentStatus_PaymentStatusId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_PspAccount_PspAccountId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentLog_Payment_PaymentId",
                table: "PaymentLog");

            migrationBuilder.DropForeignKey(
                name: "FK_PspAccount_Account_AccountId",
                table: "PspAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_PspAccount_Psp_PspId",
                table: "PspAccount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PspAccount",
                table: "PspAccount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Psp",
                table: "Psp");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentStatus",
                table: "PaymentStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentLog",
                table: "PaymentLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payment",
                table: "Payment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customer",
                table: "Customer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Account",
                table: "Account");

            migrationBuilder.RenameTable(
                name: "PspAccount",
                newName: "PspAccounts");

            migrationBuilder.RenameTable(
                name: "Psp",
                newName: "Psps");

            migrationBuilder.RenameTable(
                name: "PaymentStatus",
                newName: "PaymentStatuss");

            migrationBuilder.RenameTable(
                name: "PaymentLog",
                newName: "PaymentLogs");

            migrationBuilder.RenameTable(
                name: "Payment",
                newName: "Payments");

            migrationBuilder.RenameTable(
                name: "Customer",
                newName: "Customers");

            migrationBuilder.RenameTable(
                name: "Account",
                newName: "Accounts");

            migrationBuilder.RenameIndex(
                name: "IX_PspAccount_PspId",
                table: "PspAccounts",
                newName: "IX_PspAccounts_PspId");

            migrationBuilder.RenameIndex(
                name: "IX_PspAccount_AccountId",
                table: "PspAccounts",
                newName: "IX_PspAccounts_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentLog_PaymentId",
                table: "PaymentLogs",
                newName: "IX_PaymentLogs_PaymentId");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_PspAccountId",
                table: "Payments",
                newName: "IX_Payments_PspAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_PaymentStatusId",
                table: "Payments",
                newName: "IX_Payments_PaymentStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Account_CustomerId",
                table: "Accounts",
                newName: "IX_Accounts_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PspAccounts",
                table: "PspAccounts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Psps",
                table: "Psps",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentStatuss",
                table: "PaymentStatuss",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentLogs",
                table: "PaymentLogs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payments",
                table: "Payments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Customers_CustomerId",
                table: "Accounts",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentLogs_Payments_PaymentId",
                table: "PaymentLogs",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentStatuss_PaymentStatusId",
                table: "Payments",
                column: "PaymentStatusId",
                principalTable: "PaymentStatuss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PspAccounts_PspAccountId",
                table: "Payments",
                column: "PspAccountId",
                principalTable: "PspAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PspAccounts_Accounts_AccountId",
                table: "PspAccounts",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PspAccounts_Psps_PspId",
                table: "PspAccounts",
                column: "PspId",
                principalTable: "Psps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
