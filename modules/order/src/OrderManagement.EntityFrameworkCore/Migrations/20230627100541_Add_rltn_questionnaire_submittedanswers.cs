using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class Addrltnquestionnairesubmittedanswers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionnaireId",
                table: "SubmittedAnswers",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "IX_SubmittedAnswers_QuestionnaireId",
                table: "SubmittedAnswers",
                column: "QuestionnaireId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubmittedAnswers_Questionnaire_QuestionnaireId",
                table: "SubmittedAnswers",
                column: "QuestionnaireId",
                principalTable: "Questionnaire",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmittedAnswers_Questionnaire_QuestionnaireId",
                table: "SubmittedAnswers");

            migrationBuilder.DropIndex(
                name: "IX_SubmittedAnswers_QuestionnaireId",
                table: "SubmittedAnswers");

            migrationBuilder.DropColumn(
                name: "QuestionnaireId",
                table: "SubmittedAnswers");

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
        }
    }
}
