using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class FixMisspelling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmitedAnswers_QuestionnaireAnswer_AnswerId",
                table: "SubmitedAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubmitedAnswers",
                table: "SubmitedAnswers");

            migrationBuilder.RenameTable(
                name: "SubmitedAnswers",
                newName: "SubmittedAnswers");

            migrationBuilder.RenameIndex(
                name: "IX_SubmitedAnswers_AnswerId",
                table: "SubmittedAnswers",
                newName: "IX_SubmittedAnswers_AnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubmittedAnswers",
                table: "SubmittedAnswers",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AnswerComponentType",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DeletionTime", "LastModificationTime" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AnswerComponentType",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DeletionTime", "LastModificationTime" },
                values: new object[] { null, null });

            migrationBuilder.AddForeignKey(
                name: "FK_SubmittedAnswers_QuestionnaireAnswer_AnswerId",
                table: "SubmittedAnswers",
                column: "AnswerId",
                principalTable: "QuestionnaireAnswer",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmittedAnswers_QuestionnaireAnswer_AnswerId",
                table: "SubmittedAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubmittedAnswers",
                table: "SubmittedAnswers");

            migrationBuilder.RenameTable(
                name: "SubmittedAnswers",
                newName: "SubmitedAnswers");

            migrationBuilder.RenameIndex(
                name: "IX_SubmittedAnswers_AnswerId",
                table: "SubmitedAnswers",
                newName: "IX_SubmitedAnswers_AnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubmitedAnswers",
                table: "SubmitedAnswers",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AnswerComponentType",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DeletionTime", "LastModificationTime" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "AnswerComponentType",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DeletionTime", "LastModificationTime" },
                values: new object[] { null, null });

            migrationBuilder.AddForeignKey(
                name: "FK_SubmitedAnswers_QuestionnaireAnswer_AnswerId",
                table: "SubmitedAnswers",
                column: "AnswerId",
                principalTable: "QuestionnaireAnswer",
                principalColumn: "Id");
        }
    }
}
