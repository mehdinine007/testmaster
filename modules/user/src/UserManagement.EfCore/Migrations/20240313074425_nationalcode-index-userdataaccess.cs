using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserManagement.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class nationalcodeindexuserdataaccess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserDataAccess_Nationalcode",
                table: "UserDataAccess",
                column: "Nationalcode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserDataAccess_Nationalcode",
                table: "UserDataAccess");
        }
    }
}
