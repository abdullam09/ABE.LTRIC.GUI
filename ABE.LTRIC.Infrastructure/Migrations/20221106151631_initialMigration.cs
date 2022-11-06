using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABE.LTRIC.Infrastructure.Migrations
{
    public partial class initialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Docs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    DocNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpectedDueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrincipleAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEnded = table.Column<bool>(type: "bit", nullable: false),
                    IsODEnded = table.Column<bool>(type: "bit", nullable: false),
                    ODDueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Docs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Docs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocDtls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    DocId = table.Column<int>(type: "int", nullable: false),
                    RecordDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RefundDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EarlySattleToBank = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LTR_Period = table.Column<int>(type: "int", nullable: false),
                    OD_Period = table.Column<int>(type: "int", nullable: false),
                    LTR_Intrest = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OD_Interest = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LTR_Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OD_Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total_Intrest = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalChargeToBank = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalChargeToCompany = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocDtls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocDtls_Docs_DocId",
                        column: x => x.DocId,
                        principalTable: "Docs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocDtls_DocId",
                table: "DocDtls",
                column: "DocId");

            migrationBuilder.CreateIndex(
                name: "IX_Docs_CompanyId",
                table: "Docs",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocDtls");

            migrationBuilder.DropTable(
                name: "Docs");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
