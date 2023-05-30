using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTableReferenceAgencyProvince : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agency_City_CityId",
                table: "Agency");

            migrationBuilder.RenameColumn(
                name: "CityId",
                table: "Agency",
                newName: "ProvinceId");

            migrationBuilder.RenameIndex(
                name: "IX_Agency_CityId",
                table: "Agency",
                newName: "IX_Agency_ProvinceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agency_Province_ProvinceId",
                table: "Agency",
                column: "ProvinceId",
                principalSchema: "aucbase",
                principalTable: "Province",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agency_Province_ProvinceId",
                table: "Agency");

            migrationBuilder.RenameColumn(
                name: "ProvinceId",
                table: "Agency",
                newName: "CityId");

            migrationBuilder.RenameIndex(
                name: "IX_Agency_ProvinceId",
                table: "Agency",
                newName: "IX_Agency_CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agency_City_CityId",
                table: "Agency",
                column: "CityId",
                principalSchema: "aucbase",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
