using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class addSeasonAllocationtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SeasonAllocationId",
                table: "CustomerOrder",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SeasonAllocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeasonId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_SeasonAllocation", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrder_SeasonAllocationId",
                table: "CustomerOrder",
                column: "SeasonAllocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerOrder_SeasonAllocation_SeasonAllocationId",
                table: "CustomerOrder",
                column: "SeasonAllocationId",
                principalTable: "SeasonAllocation",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerOrder_SeasonAllocation_SeasonAllocationId",
                table: "CustomerOrder");

            migrationBuilder.DropTable(
                name: "SeasonAllocation");

            migrationBuilder.DropIndex(
                name: "IX_CustomerOrder_SeasonAllocationId",
                table: "CustomerOrder");

            migrationBuilder.DropColumn(
                name: "SeasonAllocationId",
                table: "CustomerOrder");
        }
    }
}
