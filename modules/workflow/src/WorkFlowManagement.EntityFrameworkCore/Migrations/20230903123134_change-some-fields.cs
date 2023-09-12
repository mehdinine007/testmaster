using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkFlowManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class changesomefields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "Flow",
                table: "OrganizationPositions",
                newName: "PersonId");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                schema: "Flow",
                table: "Processes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId1",
                schema: "Flow",
                table: "Processes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "Flow",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Processes_ParentId1",
                schema: "Flow",
                table: "Processes",
                column: "ParentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Processes_Processes_ParentId1",
                schema: "Flow",
                table: "Processes",
                column: "ParentId1",
                principalSchema: "Flow",
                principalTable: "Processes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Processes_Processes_ParentId1",
                schema: "Flow",
                table: "Processes");

            migrationBuilder.DropIndex(
                name: "IX_Processes_ParentId1",
                schema: "Flow",
                table: "Processes");

            migrationBuilder.DropColumn(
                name: "ParentId",
                schema: "Flow",
                table: "Processes");

            migrationBuilder.DropColumn(
                name: "ParentId1",
                schema: "Flow",
                table: "Processes");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                schema: "Flow",
                table: "OrganizationPositions",
                newName: "UserId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "Flow",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
