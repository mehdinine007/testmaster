using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkFlowManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class addorganizationpositionprocess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganizationPositionId",
                schema: "Flow",
                table: "Processes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Processes_OrganizationPositionId",
                schema: "Flow",
                table: "Processes",
                column: "OrganizationPositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Processes_OrganizationPositions_OrganizationPositionId",
                schema: "Flow",
                table: "Processes",
                column: "OrganizationPositionId",
                principalSchema: "Flow",
                principalTable: "OrganizationPositions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Processes_OrganizationPositions_OrganizationPositionId",
                schema: "Flow",
                table: "Processes");

            migrationBuilder.DropIndex(
                name: "IX_Processes_OrganizationPositionId",
                schema: "Flow",
                table: "Processes");

            migrationBuilder.DropColumn(
                name: "OrganizationPositionId",
                schema: "Flow",
                table: "Processes");
        }
    }
}
