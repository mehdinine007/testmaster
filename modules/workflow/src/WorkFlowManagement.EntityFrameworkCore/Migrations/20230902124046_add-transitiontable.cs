using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkFlowManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class addtransitiontable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transitions",
                schema: "Flow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivitySourceId = table.Column<int>(type: "int", nullable: false),
                    ActivityTargetId = table.Column<int>(type: "int", nullable: false),
                    SchemeId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_Transitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transitions_Activities_ActivitySourceId",
                        column: x => x.ActivitySourceId,
                        principalSchema: "Flow",
                        principalTable: "Activities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transitions_Activities_ActivityTargetId",
                        column: x => x.ActivityTargetId,
                        principalSchema: "Flow",
                        principalTable: "Activities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transitions_Schemes_SchemeId",
                        column: x => x.SchemeId,
                        principalSchema: "Flow",
                        principalTable: "Schemes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transitions_ActivitySourceId",
                schema: "Flow",
                table: "Transitions",
                column: "ActivitySourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Transitions_ActivityTargetId",
                schema: "Flow",
                table: "Transitions",
                column: "ActivityTargetId");

            migrationBuilder.CreateIndex(
                name: "IX_Transitions_SchemeId",
                schema: "Flow",
                table: "Transitions",
                column: "SchemeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transitions",
                schema: "Flow");
        }
    }
}
