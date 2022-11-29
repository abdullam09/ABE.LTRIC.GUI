using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABE.LTRIC.Infrastructure.Migrations
{
    public partial class AddRecoveredFromCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "RecoveredFromCompany",
                table: "DocDtls",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecoveredFromCompany",
                table: "DocDtls");
        }
    }
}
