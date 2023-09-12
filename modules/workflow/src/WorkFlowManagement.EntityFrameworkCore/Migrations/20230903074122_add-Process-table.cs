using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkFlowManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class addProcesstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Processes",
                schema: "Flow",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SchemeId = table.Column<int>(type: "int", nullable: false),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    PreviousActivityId = table.Column<int>(type: "int", nullable: true),
                    OrganizationChartId = table.Column<int>(type: "int", nullable: false),
                    CreatedOrganizationChartId = table.Column<int>(type: "int", nullable: false),
                    PreviousOrganizationChartId = table.Column<int>(type: "int", nullable: true),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreviousPersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedPersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_Processes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Processes_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalSchema: "Flow",
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Processes_Activities_PreviousActivityId",
                        column: x => x.PreviousActivityId,
                        principalSchema: "Flow",
                        principalTable: "Activities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Processes_OrganizationCharts_CreatedOrganizationChartId",
                        column: x => x.CreatedOrganizationChartId,
                        principalSchema: "Flow",
                        principalTable: "OrganizationCharts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Processes_OrganizationCharts_OrganizationChartId",
                        column: x => x.OrganizationChartId,
                        principalSchema: "Flow",
                        principalTable: "OrganizationCharts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Processes_OrganizationCharts_PreviousOrganizationChartId",
                        column: x => x.PreviousOrganizationChartId,
                        principalSchema: "Flow",
                        principalTable: "OrganizationCharts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Processes_Schemes_SchemeId",
                        column: x => x.SchemeId,
                        principalSchema: "Flow",
                        principalTable: "Schemes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Processes_ActivityId",
                schema: "Flow",
                table: "Processes",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_CreatedOrganizationChartId",
                schema: "Flow",
                table: "Processes",
                column: "CreatedOrganizationChartId");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_OrganizationChartId",
                schema: "Flow",
                table: "Processes",
                column: "OrganizationChartId");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_PreviousActivityId",
                schema: "Flow",
                table: "Processes",
                column: "PreviousActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_PreviousOrganizationChartId",
                schema: "Flow",
                table: "Processes",
                column: "PreviousOrganizationChartId");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_SchemeId",
                schema: "Flow",
                table: "Processes",
                column: "SchemeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Processes",
                schema: "Flow");
        }
    }
}
