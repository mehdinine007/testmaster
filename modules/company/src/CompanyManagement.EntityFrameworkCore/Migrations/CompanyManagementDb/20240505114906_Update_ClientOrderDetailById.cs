using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.EfCore.Migrations.CompanyManagementDb
{
    /// <inheritdoc />
    public partial class Update_ClientOrderDetailById : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RelatedToOrganization",
                table: "ClientsOrderDetailByCompany",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelatedToOrganization",
                table: "ClientsOrderDetailByCompany");
        }
    }
}
