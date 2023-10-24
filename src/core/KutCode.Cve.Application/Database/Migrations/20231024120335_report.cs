using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KutCode.Cve.Application.Database.Migrations
{
    /// <inheritdoc />
    public partial class report : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "report_request",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    custom_name = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<int>(type: "integer", nullable: false),
                    search_strategy = table.Column<int>(type: "integer", nullable: false),
                    sources = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_report_request", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "report_request_cve",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    cve_year = table.Column<int>(type: "integer", nullable: false),
                    cve_cna_number = table.Column<string>(type: "text", nullable: false),
                    platform = table.Column<string>(type: "text", nullable: false),
                    software = table.Column<string>(type: "text", nullable: false),
                    report_request_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_report_request_cve", x => x.id);
                    table.ForeignKey(
                        name: "FK_report_request_cve_report_request_report_request_id",
                        column: x => x.report_request_id,
                        principalTable: "report_request",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_report_request_cve_report_request_id",
                table: "report_request_cve",
                column: "report_request_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "report_request_cve");

            migrationBuilder.DropTable(
                name: "report_request");
        }
    }
}
