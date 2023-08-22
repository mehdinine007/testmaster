using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class MakeUserIdLong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "SubmitedAnswers",
                type: "bigint",
                nullable: false,
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "SubmitedAnswers",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

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
