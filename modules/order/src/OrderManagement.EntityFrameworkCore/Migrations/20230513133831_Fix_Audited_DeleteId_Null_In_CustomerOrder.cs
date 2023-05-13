using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class FixAuditedDeleteIdNullInCustomerOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvocacyUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WhiteLists",
                table: "WhiteLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRejectionAdvocacies",
                table: "UserRejectionAdvocacies");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Year");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "Year");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Season_Company_CarTip");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "Season_Company_CarTip");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "ESaleType");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "ESaleType");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "CarType");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "CarType");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "CarTip_Gallery_Mapping");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "CarTip_Gallery_Mapping");

            migrationBuilder.RenameTable(
                name: "WhiteLists",
                newName: "WhiteList");

            migrationBuilder.RenameTable(
                name: "UserRejectionAdvocacies",
                newName: "UserRejectionAdvocacy");

            migrationBuilder.RenameIndex(
                name: "IX_WhiteLists_NationalCode_WhiteListType",
                table: "WhiteList",
                newName: "IX_WhiteList_NationalCode_WhiteListType");

            migrationBuilder.RenameIndex(
                name: "IX_WhiteLists_NationalCode",
                table: "WhiteList",
                newName: "IX_WhiteList_NationalCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WhiteList",
                table: "WhiteList",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRejectionAdvocacy",
                table: "UserRejectionAdvocacy",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AdvocacyUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nationalcode = table.Column<string>(type: "NCHAR(10)", nullable: false),
                    bankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    dateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    accountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    shabaNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    BanksId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_AdvocacyUser", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvocacyUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WhiteList",
                table: "WhiteList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRejectionAdvocacy",
                table: "UserRejectionAdvocacy");

            migrationBuilder.RenameTable(
                name: "WhiteList",
                newName: "WhiteLists");

            migrationBuilder.RenameTable(
                name: "UserRejectionAdvocacy",
                newName: "UserRejectionAdvocacies");

            migrationBuilder.RenameIndex(
                name: "IX_WhiteList_NationalCode_WhiteListType",
                table: "WhiteLists",
                newName: "IX_WhiteLists_NationalCode_WhiteListType");

            migrationBuilder.RenameIndex(
                name: "IX_WhiteList_NationalCode",
                table: "WhiteLists",
                newName: "IX_WhiteLists_NationalCode");

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Year",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "Year",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Seasons",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "Seasons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Season_Company_CarTip",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "Season_Company_CarTip",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "ESaleType",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "ESaleType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "CarType",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "CarType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "CarTip_Gallery_Mapping",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "CarTip_Gallery_Mapping",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WhiteLists",
                table: "WhiteLists",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRejectionAdvocacies",
                table: "UserRejectionAdvocacies",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AdvocacyUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BanksId = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    accountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    nationalcode = table.Column<string>(type: "NCHAR(10)", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    shabaNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                }
                //,
                //constraints: table =>
                //{
                //    table.PrimaryKey("PK_AdvocacyUsers", x => x.Id);
                //}
                );
        }
    }
}
