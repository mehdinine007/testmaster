using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkFlowManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class changeinboxnametable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inboxes_Inboxes_ParentId",
                schema: "Flow",
                table: "Inboxes");

            migrationBuilder.DropForeignKey(
                name: "FK_Inboxes_OrganizationCharts_OrganizationChartId",
                schema: "Flow",
                table: "Inboxes");

            migrationBuilder.DropForeignKey(
                name: "FK_Inboxes_OrganizationPositions_OrganizationPositionId",
                schema: "Flow",
                table: "Inboxes");

            migrationBuilder.DropForeignKey(
                name: "FK_Inboxes_Processes_ProcessId",
                schema: "Flow",
                table: "Inboxes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Inboxes",
                schema: "Flow",
                table: "Inboxes");

            migrationBuilder.RenameTable(
                name: "Inboxes",
                schema: "Flow",
                newName: "Inbox",
                newSchema: "Flow");

            //migrationBuilder.RenameIndex(
            //    name: "IX_Inboxes_ProcessId",
            //    schema: "Flow",
            //    table: "Inbox",
            //    newName: "IX_Inbox_ProcessId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_Inboxes_ParentId",
            //    schema: "Flow",
            //    table: "Inbox",
            //    newName: "IX_Inbox_ParentId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_Inboxes_OrganizationPositionId",
            //    schema: "Flow",
            //    table: "Inbox",
            //    newName: "IX_Inbox_OrganizationPositionId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_Inboxes_OrganizationChartId",
            //    schema: "Flow",
            //    table: "Inbox",
            //    newName: "IX_Inbox_OrganizationChartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Inbox",
                schema: "Flow",
                table: "Inbox",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inbox_Inbox_ParentId",
                schema: "Flow",
                table: "Inbox",
                column: "ParentId",
                principalSchema: "Flow",
                principalTable: "Inbox",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inbox_OrganizationCharts_OrganizationChartId",
                schema: "Flow",
                table: "Inbox",
                column: "OrganizationChartId",
                principalSchema: "Flow",
                principalTable: "OrganizationCharts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inbox_OrganizationPositions_OrganizationPositionId",
                schema: "Flow",
                table: "Inbox",
                column: "OrganizationPositionId",
                principalSchema: "Flow",
                principalTable: "OrganizationPositions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inbox_Processes_ProcessId",
                schema: "Flow",
                table: "Inbox",
                column: "ProcessId",
                principalSchema: "Flow",
                principalTable: "Processes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inbox_Inbox_ParentId",
                schema: "Flow",
                table: "Inbox");

            migrationBuilder.DropForeignKey(
                name: "FK_Inbox_OrganizationCharts_OrganizationChartId",
                schema: "Flow",
                table: "Inbox");

            migrationBuilder.DropForeignKey(
                name: "FK_Inbox_OrganizationPositions_OrganizationPositionId",
                schema: "Flow",
                table: "Inbox");

            migrationBuilder.DropForeignKey(
                name: "FK_Inbox_Processes_ProcessId",
                schema: "Flow",
                table: "Inbox");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Inbox",
                schema: "Flow",
                table: "Inbox");

            migrationBuilder.RenameTable(
                name: "Inbox",
                schema: "Flow",
                newName: "Inboxes",
                newSchema: "Flow");

            migrationBuilder.RenameIndex(
                name: "IX_Inbox_ProcessId",
                schema: "Flow",
                table: "Inboxes",
                newName: "IX_Inboxes_ProcessId");

            migrationBuilder.RenameIndex(
                name: "IX_Inbox_ParentId",
                schema: "Flow",
                table: "Inboxes",
                newName: "IX_Inboxes_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Inbox_OrganizationPositionId",
                schema: "Flow",
                table: "Inboxes",
                newName: "IX_Inboxes_OrganizationPositionId");

            migrationBuilder.RenameIndex(
                name: "IX_Inbox_OrganizationChartId",
                schema: "Flow",
                table: "Inboxes",
                newName: "IX_Inboxes_OrganizationChartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Inboxes",
                schema: "Flow",
                table: "Inboxes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inboxes_Inboxes_ParentId",
                schema: "Flow",
                table: "Inboxes",
                column: "ParentId",
                principalSchema: "Flow",
                principalTable: "Inboxes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inboxes_OrganizationCharts_OrganizationChartId",
                schema: "Flow",
                table: "Inboxes",
                column: "OrganizationChartId",
                principalSchema: "Flow",
                principalTable: "OrganizationCharts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inboxes_OrganizationPositions_OrganizationPositionId",
                schema: "Flow",
                table: "Inboxes",
                column: "OrganizationPositionId",
                principalSchema: "Flow",
                principalTable: "OrganizationPositions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inboxes_Processes_ProcessId",
                schema: "Flow",
                table: "Inboxes",
                column: "ProcessId",
                principalSchema: "Flow",
                principalTable: "Processes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
