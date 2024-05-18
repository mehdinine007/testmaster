using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class addfieldsagency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SaleStatus",
                table: "SaleSchema",
                newName: "Code");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Agency",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AgencyType",
                table: "Agency",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Agency",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Agency",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "Agency",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "Agency",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Agency",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Visible",
                table: "Agency",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Agency_CityId",
                table: "Agency",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agency_City_CityId",
                table: "Agency",
                column: "CityId",
                principalSchema: "aucbase",
                principalTable: "City",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agency_City_CityId",
                table: "Agency");

            migrationBuilder.DropIndex(
                name: "IX_Agency_CityId",
                table: "Agency");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Agency");

            migrationBuilder.DropColumn(
                name: "AgencyType",
                table: "Agency");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Agency");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Agency");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Agency");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Agency");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Agency");

            migrationBuilder.DropColumn(
                name: "Visible",
                table: "Agency");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "SaleSchema",
                newName: "SaleStatus");
        }
    }
}
