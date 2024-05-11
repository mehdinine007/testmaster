using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class addSeasonAllocationId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeasonId",
                table: "SaleDetailAllocation");

            migrationBuilder.AddColumn<int>(
                name: "SeasonAllocationId",
                table: "SaleDetailAllocation",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetailAllocation_SeasonAllocationId",
                table: "SaleDetailAllocation",
                column: "SeasonAllocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleDetailAllocation_SeasonAllocation_SeasonAllocationId",
                table: "SaleDetailAllocation",
                column: "SeasonAllocationId",
                principalTable: "SeasonAllocation",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleDetailAllocation_SeasonAllocation_SeasonAllocationId",
                table: "SaleDetailAllocation");

            migrationBuilder.DropIndex(
                name: "IX_SaleDetailAllocation_SeasonAllocationId",
                table: "SaleDetailAllocation");

            migrationBuilder.DropColumn(
                name: "SeasonAllocationId",
                table: "SaleDetailAllocation");

            migrationBuilder.AddColumn<int>(
                name: "SeasonId",
                table: "SaleDetailAllocation",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
