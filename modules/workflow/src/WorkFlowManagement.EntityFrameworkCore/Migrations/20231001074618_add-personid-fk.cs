using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkFlowManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class addpersonidfk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Processes_PersonId",
                schema: "Flow",
                table: "Processes",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_PreviousPersonId",
                schema: "Flow",
                table: "Processes",
                column: "PreviousPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Inbox_PersonId",
                schema: "Flow",
                table: "Inbox",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inbox_Person_PersonId",
                schema: "Flow",
                table: "Inbox",
                column: "PersonId",
                principalSchema: "Flow",
                principalTable: "Person",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Processes_Person_PersonId",
                schema: "Flow",
                table: "Processes",
                column: "PersonId",
                principalSchema: "Flow",
                principalTable: "Person",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Processes_Person_PreviousPersonId",
                schema: "Flow",
                table: "Processes",
                column: "PreviousPersonId",
                principalSchema: "Flow",
                principalTable: "Person",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inbox_Person_PersonId",
                schema: "Flow",
                table: "Inbox");

            migrationBuilder.DropForeignKey(
                name: "FK_Processes_Person_PersonId",
                schema: "Flow",
                table: "Processes");

            migrationBuilder.DropForeignKey(
                name: "FK_Processes_Person_PreviousPersonId",
                schema: "Flow",
                table: "Processes");

            migrationBuilder.DropIndex(
                name: "IX_Processes_PersonId",
                schema: "Flow",
                table: "Processes");

            migrationBuilder.DropIndex(
                name: "IX_Processes_PreviousPersonId",
                schema: "Flow",
                table: "Processes");

            migrationBuilder.DropIndex(
                name: "IX_Inbox_PersonId",
                schema: "Flow",
                table: "Inbox");
        }
    }
}
