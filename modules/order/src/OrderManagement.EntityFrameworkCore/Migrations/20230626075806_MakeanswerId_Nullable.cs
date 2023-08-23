using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class MakeanswerIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmitedAnswers_QuestionnaireAnswer_AnswerId",
                table: "SubmitedAnswers");

            migrationBuilder.AlterColumn<int>(
                name: "AnswerId",
                table: "SubmitedAnswers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmitedAnswers_QuestionnaireAnswer_AnswerId",
                table: "SubmitedAnswers");

            migrationBuilder.AlterColumn<int>(
                name: "AnswerId",
                table: "SubmitedAnswers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
