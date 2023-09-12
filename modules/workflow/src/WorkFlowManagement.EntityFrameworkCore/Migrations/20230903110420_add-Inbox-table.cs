using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkFlowManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class addInboxtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inboxes",
                schema: "Flow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationChartId = table.Column<int>(type: "int", nullable: false),
                    OrganizationPositionId = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ReferenceDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_Inboxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inboxes_Inboxes_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Flow",
                        principalTable: "Inboxes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Inboxes_OrganizationCharts_OrganizationChartId",
                        column: x => x.OrganizationChartId,
                        principalSchema: "Flow",
                        principalTable: "OrganizationCharts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Inboxes_OrganizationPositions_OrganizationPositionId",
                        column: x => x.OrganizationPositionId,
                        principalSchema: "Flow",
                        principalTable: "OrganizationPositions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Inboxes_Processes_ProcessId",
                        column: x => x.ProcessId,
                        principalSchema: "Flow",
                        principalTable: "Processes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inboxes_OrganizationChartId",
                schema: "Flow",
                table: "Inboxes",
                column: "OrganizationChartId");

            migrationBuilder.CreateIndex(
                name: "IX_Inboxes_OrganizationPositionId",
                schema: "Flow",
                table: "Inboxes",
                column: "OrganizationPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Inboxes_ParentId",
                schema: "Flow",
                table: "Inboxes",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Inboxes_ProcessId",
                schema: "Flow",
                table: "Inboxes",
                column: "ProcessId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inboxes",
                schema: "Flow");
        }
    }
}
