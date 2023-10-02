using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations.CompanyManagementDb
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientsOrderDetailByCompany",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NationalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SaleType = table.Column<int>(type: "int", maxLength: 150, nullable: true),
                    ModelType = table.Column<int>(type: "int", nullable: true),
                    InviteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContRowId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContRowIdDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Vin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BodyNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CooperateBenefit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CancelBenefit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DelayBenefit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinalPrice = table.Column<long>(type: "bigint", nullable: true),
                    CarDesc = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CarCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    IntroductionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FactorDate = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_ClientsOrderDetailByCompany", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyProduction",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CarDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductionCount = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_CompanyProduction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyPaypaidPrices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TranDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PayedPrice = table.Column<long>(type: "bigint", nullable: false),
                    ClientsOrderDetailByCompanyId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_CompanyPaypaidPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyPaypaidPrices_ClientsOrderDetailByCompany_ClientsOrderDetailByCompanyId",
                        column: x => x.ClientsOrderDetailByCompanyId,
                        principalTable: "ClientsOrderDetailByCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanySaleCallDates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTurnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTurnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClientsOrderDetailByCompanyId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_CompanySaleCallDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanySaleCallDates_ClientsOrderDetailByCompany_ClientsOrderDetailByCompanyId",
                        column: x => x.ClientsOrderDetailByCompanyId,
                        principalTable: "ClientsOrderDetailByCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyPaypaidPrices_ClientsOrderDetailByCompanyId",
                table: "CompanyPaypaidPrices",
                column: "ClientsOrderDetailByCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanySaleCallDates_ClientsOrderDetailByCompanyId",
                table: "CompanySaleCallDates",
                column: "ClientsOrderDetailByCompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyPaypaidPrices");

            migrationBuilder.DropTable(
                name: "CompanyProduction");

            migrationBuilder.DropTable(
                name: "CompanySaleCallDates");

            migrationBuilder.DropTable(
                name: "ClientsOrderDetailByCompany");
        }
    }
}
