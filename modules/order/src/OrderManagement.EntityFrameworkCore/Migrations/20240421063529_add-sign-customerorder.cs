using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class addsigncustomerorder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SignStatus",
                table: "CustomerOrder",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SignTicketId",
                table: "CustomerOrder",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SignStatus",
                table: "CustomerOrder");

            migrationBuilder.DropColumn(
                name: "SignTicketId",
                table: "CustomerOrder");
        }
    }
}
