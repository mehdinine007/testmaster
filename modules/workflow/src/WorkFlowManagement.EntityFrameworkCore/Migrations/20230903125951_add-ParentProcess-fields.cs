using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkFlowManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class addParentProcessfields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                schema: "Flow",
                table: "Processes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Processes_ParentId",
                schema: "Flow",
                table: "Processes",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Processes_Processes_ParentId",
                schema: "Flow",
                table: "Processes",
                column: "ParentId",
                principalSchema: "Flow",
                principalTable: "Processes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Processes_Processes_ParentId",
                schema: "Flow",
                table: "Processes");

            migrationBuilder.DropIndex(
                name: "IX_Processes_ParentId",
                schema: "Flow",
                table: "Processes");

            migrationBuilder.DropColumn(
                name: "ParentId",
                schema: "Flow",
                table: "Processes");
        }
    }
}
