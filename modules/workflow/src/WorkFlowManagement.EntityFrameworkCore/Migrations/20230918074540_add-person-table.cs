using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkFlowManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class addpersontable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Person",
                schema: "Flow",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "NVARCHAR(300)", nullable: true),
                    NationalCode = table.Column<string>(type: "NCHAR(10)", nullable: true),
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
                    table.PrimaryKey("PK_Person", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationPositions_PersonId",
                schema: "Flow",
                table: "OrganizationPositions",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationPositions_Person_PersonId",
                schema: "Flow",
                table: "OrganizationPositions",
                column: "PersonId",
                principalSchema: "Flow",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationPositions_Person_PersonId",
                schema: "Flow",
                table: "OrganizationPositions");

            migrationBuilder.DropTable(
                name: "Person",
                schema: "Flow");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationPositions_PersonId",
                schema: "Flow",
                table: "OrganizationPositions");
        }
    }
}
