using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkFlowManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class changetablenameRoleOrganizationChart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleCharts",
                schema: "Flow");

            migrationBuilder.CreateTable(
                name: "RoleOrganizationChart",
                schema: "Flow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    OrganizationChartId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_RoleOrganizationChart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleOrganizationChart_OrganizationCharts_OrganizationChartId",
                        column: x => x.OrganizationChartId,
                        principalSchema: "Flow",
                        principalTable: "OrganizationCharts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleOrganizationChart_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Flow",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleOrganizationChart_OrganizationChartId",
                schema: "Flow",
                table: "RoleOrganizationChart",
                column: "OrganizationChartId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleOrganizationChart_RoleId",
                schema: "Flow",
                table: "RoleOrganizationChart",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleOrganizationChart",
                schema: "Flow");

            migrationBuilder.CreateTable(
                name: "RoleCharts",
                schema: "Flow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationChartId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleCharts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleCharts_OrganizationCharts_OrganizationChartId",
                        column: x => x.OrganizationChartId,
                        principalSchema: "Flow",
                        principalTable: "OrganizationCharts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleCharts_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Flow",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleCharts_OrganizationChartId",
                schema: "Flow",
                table: "RoleCharts",
                column: "OrganizationChartId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleCharts_RoleId",
                schema: "Flow",
                table: "RoleCharts",
                column: "RoleId");
        }
    }
}
