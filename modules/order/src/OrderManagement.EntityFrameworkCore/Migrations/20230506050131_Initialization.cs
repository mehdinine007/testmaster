using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations
{
    public partial class Initialization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdvocacyUsers",
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
                    table.PrimaryKey("PK_AdvocacyUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ESaleType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
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
                    table.PrimaryKey("PK_ESaleType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExternalApiLogResult",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NationalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Resopnse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    SaleId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_ExternalApiLogResult", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExternalApiResponsLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Result = table.Column<bool>(type: "bit", nullable: false),
                    NationalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    SaleId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_ExternalApiResponsLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gallery",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Gallery", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Method = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Message = table.Column<string>(type: "VARCHAR(max)", nullable: true),
                    Type = table.Column<short>(type: "smallint", nullable: false),
                    Servername = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    Ip = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
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
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WhiteLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NationalCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    WhiteListType = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_WhiteLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Year",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
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
                    table.PrimaryKey("PK_Year", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bank_Gallery_LogoId",
                        column: x => x.LogoId,
                        principalTable: "Gallery",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoId = table.Column<int>(type: "int", nullable: true),
                    LogoInPageId = table.Column<int>(type: "int", nullable: true),
                    BannerId = table.Column<int>(type: "int", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
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
                    table.PrimaryKey("PK_Company", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Company_Gallery_BannerId",
                        column: x => x.BannerId,
                        principalTable: "Gallery",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Company_Gallery_LogoId",
                        column: x => x.LogoId,
                        principalTable: "Gallery",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Company_Gallery_LogoInPageId",
                        column: x => x.LogoInPageId,
                        principalTable: "Gallery",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserRejectionFromBank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nationalcode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    bankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    price = table.Column<decimal>(type: "decimal(15)", nullable: false),
                    dateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    accountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    shabaNumber = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    BanksId = table.Column<int>(type: "int", nullable: true),
                    CarMaker = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_UserRejectionFromBank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRejectionFromBank_Bank_BanksId",
                        column: x => x.BanksId,
                        principalTable: "Bank",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AdvocacyUsersFromBank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nationalcode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    bankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    price = table.Column<decimal>(type: "decimal(15)", nullable: false),
                    dateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    accountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    shabaNumber = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    BanksId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_AdvocacyUsersFromBank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdvocacyUsersFromBank_Bank_BanksId",
                        column: x => x.BanksId,
                        principalTable: "Bank",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdvocacyUsersFromBank_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CarFamily",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_CarFamily", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarFamily_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CarFamilyId = table.Column<int>(type: "int", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
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
                    table.PrimaryKey("PK_CarType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarType_CarFamily_CarFamilyId",
                        column: x => x.CarFamilyId,
                        principalTable: "CarFamily",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarTip",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CarTypeId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_CarTip", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarTip_CarType_CarTypeId",
                        column: x => x.CarTypeId,
                        principalTable: "CarType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarTip_Gallery_Mapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GalleryId = table.Column<int>(type: "int", nullable: false),
                    CarTipId = table.Column<int>(type: "int", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
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
                    table.PrimaryKey("PK_CarTip_Gallery_Mapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarTip_Gallery_Mapping_CarTip_CarTipId",
                        column: x => x.CarTipId,
                        principalTable: "CarTip",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CarTip_Gallery_Mapping_Gallery_GalleryId",
                        column: x => x.GalleryId,
                        principalTable: "Gallery",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SaleDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CircularSaleCode = table.Column<int>(type: "int", nullable: false),
                    SaleId = table.Column<int>(type: "int", nullable: false),
                    SalePlanCode = table.Column<int>(type: "int", nullable: false),
                    SalePlanDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CarTipId = table.Column<int>(type: "int", nullable: false),
                    ManufactureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SalePlanStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SalePlanEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CarDeliverDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SaleTypeCapacity = table.Column<int>(type: "int", nullable: false),
                    CoOperatingProfitPercentage = table.Column<double>(type: "float", nullable: false),
                    RefuseProfitPercentage = table.Column<double>(type: "float", nullable: false),
                    ESaleTypeId = table.Column<int>(type: "int", nullable: false),
                    CarFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinimumAmountOfProxyDeposit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeliverDaysCount = table.Column<int>(type: "int", nullable: false),
                    Visible = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
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
                    table.PrimaryKey("PK_SaleDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleDetail_CarTip_CarTipId",
                        column: x => x.CarTipId,
                        principalTable: "CarTip",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaleDetail_ESaleType_ESaleTypeId",
                        column: x => x.ESaleTypeId,
                        principalTable: "ESaleType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Season_Company_CarTip",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    SeasonId = table.Column<int>(type: "int", nullable: false),
                    CarTipId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    EsaleTypeId = table.Column<int>(type: "int", nullable: true),
                    YearId = table.Column<int>(type: "int", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
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
                    table.PrimaryKey("PK_Season_Company_CarTip", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Season_Company_CarTip_CarTip_CarTipId",
                        column: x => x.CarTipId,
                        principalTable: "CarTip",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Season_Company_CarTip_ESaleType_EsaleTypeId",
                        column: x => x.EsaleTypeId,
                        principalTable: "ESaleType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Season_Company_CarTip_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Season_Company_CarTip_Year_YearId",
                        column: x => x.YearId,
                        principalTable: "Year",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerOrder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleDetailId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    SaleId = table.Column<int>(type: "int", nullable: false),
                    PriorityId = table.Column<int>(type: "int", nullable: true),
                    OrderStatus = table.Column<int>(type: "int", nullable: false),
                    DeliveryDateDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderRejectionStatus = table.Column<int>(type: "int", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PriorityUser = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_CustomerOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerOrder_SaleDetail_SaleDetailId",
                        column: x => x.SaleDetailId,
                        principalTable: "SaleDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvocacyUsersFromBank_BanksId",
                table: "AdvocacyUsersFromBank",
                column: "BanksId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvocacyUsersFromBank_CompanyId",
                table: "AdvocacyUsersFromBank",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvocacyUsersFromBank_nationalcode",
                table: "AdvocacyUsersFromBank",
                column: "nationalcode");

            migrationBuilder.CreateIndex(
                name: "IX_Bank_LogoId",
                table: "Bank",
                column: "LogoId",
                unique: true,
                filter: "[LogoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CarFamily_CompanyId",
                table: "CarFamily",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CarTip_CarTypeId",
                table: "CarTip",
                column: "CarTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CarTip_Gallery_Mapping_CarTipId",
                table: "CarTip_Gallery_Mapping",
                column: "CarTipId");

            migrationBuilder.CreateIndex(
                name: "IX_CarTip_Gallery_Mapping_GalleryId",
                table: "CarTip_Gallery_Mapping",
                column: "GalleryId");

            migrationBuilder.CreateIndex(
                name: "IX_CarType_CarFamilyId",
                table: "CarType",
                column: "CarFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_BannerId",
                table: "Company",
                column: "BannerId",
                unique: true,
                filter: "[BannerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Company_LogoId",
                table: "Company",
                column: "LogoId",
                unique: true,
                filter: "[LogoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Company_LogoInPageId",
                table: "Company",
                column: "LogoInPageId",
                unique: true,
                filter: "[LogoInPageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrder_SaleDetailId_UserId",
                table: "CustomerOrder",
                columns: new[] { "SaleDetailId", "UserId" },
                unique: true,
                filter: "IsDeleted = 0 and PriorityId IS NOT NULL and OrderStatus =10");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrder_SaleId_UserId_OrderStatus",
                table: "CustomerOrder",
                columns: new[] { "SaleId", "UserId", "OrderStatus" },
                unique: true,
                filter: "IsDeleted = 0 and PriorityId IS NULL and OrderStatus =10");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrder_SaleId_UserId_OrderStatus_PriorityId",
                table: "CustomerOrder",
                columns: new[] { "SaleId", "UserId", "OrderStatus", "PriorityId" },
                unique: true,
                filter: "IsDeleted = 0 and PriorityId IS NOT NULL and OrderStatus =10");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrder_UserId_OrderStatus",
                table: "CustomerOrder",
                columns: new[] { "UserId", "OrderStatus" },
                filter: "IsDeleted = 0  ");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetail_CarTipId",
                table: "SaleDetail",
                column: "CarTipId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetail_ESaleTypeId",
                table: "SaleDetail",
                column: "ESaleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetail_UID",
                table: "SaleDetail",
                column: "UID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Season_Company_CarTip_CarTipId",
                table: "Season_Company_CarTip",
                column: "CarTipId");

            migrationBuilder.CreateIndex(
                name: "IX_Season_Company_CarTip_EsaleTypeId",
                table: "Season_Company_CarTip",
                column: "EsaleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Season_Company_CarTip_SeasonId",
                table: "Season_Company_CarTip",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Season_Company_CarTip_YearId",
                table: "Season_Company_CarTip",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRejectionFromBank_BanksId",
                table: "UserRejectionFromBank",
                column: "BanksId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRejectionFromBank_nationalcode",
                table: "UserRejectionFromBank",
                column: "nationalcode",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_WhiteLists_NationalCode",
                table: "WhiteLists",
                column: "NationalCode",
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_WhiteLists_NationalCode_WhiteListType",
                table: "WhiteLists",
                columns: new[] { "NationalCode", "WhiteListType" },
                filter: "IsDeleted = 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvocacyUsers");

            migrationBuilder.DropTable(
                name: "AdvocacyUsersFromBank");

            migrationBuilder.DropTable(
                name: "CarTip_Gallery_Mapping");

            migrationBuilder.DropTable(
                name: "CustomerOrder");

            migrationBuilder.DropTable(
                name: "ExternalApiLogResult");

            migrationBuilder.DropTable(
                name: "ExternalApiResponsLog");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Season_Company_CarTip");

            migrationBuilder.DropTable(
                name: "UserRejectionFromBank");

            migrationBuilder.DropTable(
                name: "WhiteLists");

            migrationBuilder.DropTable(
                name: "SaleDetail");

            migrationBuilder.DropTable(
                name: "Seasons");

            migrationBuilder.DropTable(
                name: "Year");

            migrationBuilder.DropTable(
                name: "Bank");

            migrationBuilder.DropTable(
                name: "CarTip");

            migrationBuilder.DropTable(
                name: "ESaleType");

            migrationBuilder.DropTable(
                name: "CarType");

            migrationBuilder.DropTable(
                name: "CarFamily");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "Gallery");
        }
    }
}
