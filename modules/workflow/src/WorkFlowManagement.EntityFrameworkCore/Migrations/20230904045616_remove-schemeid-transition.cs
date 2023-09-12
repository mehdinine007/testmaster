using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkFlowManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class removeschemeidtransition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transitions_Schemes_SchemeId",
                schema: "Flow",
                table: "Transitions");

            migrationBuilder.DropIndex(
                name: "IX_Transitions_SchemeId",
                schema: "Flow",
                table: "Transitions");

            migrationBuilder.DropColumn(
                name: "SchemeId",
                schema: "Flow",
                table: "Transitions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SchemeId",
                schema: "Flow",
                table: "Transitions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transitions_SchemeId",
                schema: "Flow",
                table: "Transitions",
                column: "SchemeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transitions_Schemes_SchemeId",
                schema: "Flow",
                table: "Transitions",
                column: "SchemeId",
                principalSchema: "Flow",
                principalTable: "Schemes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
