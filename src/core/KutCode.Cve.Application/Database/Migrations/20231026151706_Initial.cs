using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KutCode.Cve.Application.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cve",
                columns: table => new
                {
                    year = table.Column<int>(type: "integer", nullable: false),
                    cna_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    short_name = table.Column<string>(type: "text", nullable: true),
                    description_en = table.Column<string>(type: "text", nullable: true),
                    description_ru = table.Column<string>(type: "text", nullable: true),
                    cvss_max_rate = table.Column<double>(type: "double precision", nullable: true),
                    locked = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cve", x => new { x.year, x.cna_number });
                });

            migrationBuilder.CreateTable(
                name: "cve_resolve_queue",
                columns: table => new
                {
                    cve_year = table.Column<int>(type: "integer", nullable: false),
                    cve_cna_number = table.Column<string>(type: "text", nullable: false),
                    resolver_code = table.Column<string>(type: "text", nullable: false),
                    update_cve = table.Column<bool>(type: "boolean", nullable: false),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    sys_created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cve_resolve_queue", x => new { x.cve_year, x.cve_cna_number, x.resolver_code });
                });

            migrationBuilder.CreateTable(
                name: "platform",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    platform_type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_platform", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "report_request",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    custom_name = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<int>(type: "integer", nullable: false),
                    search_strategy = table.Column<int>(type: "integer", nullable: false),
                    sys_created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    sources = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_report_request", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "software",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_software", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "report_request_cve",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    cve_year = table.Column<int>(type: "integer", nullable: false),
                    cve_cna_number = table.Column<string>(type: "text", nullable: false),
                    platform = table.Column<string>(type: "text", nullable: true),
                    software = table.Column<string>(type: "text", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "vulnerability_point",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    data_source_code = table.Column<string>(type: "text", nullable: false),
                    short_name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    impact = table.Column<string>(type: "text", nullable: true),
                    platform_id = table.Column<Guid>(type: "uuid", nullable: true),
                    software_id = table.Column<Guid>(type: "uuid", nullable: true),
                    cve_year = table.Column<int>(type: "integer", nullable: false),
                    cve_cna_number = table.Column<string>(type: "character varying(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vulnerability_point", x => x.id);
                    table.ForeignKey(
                        name: "FK_vulnerability_point_cve_cve_year_cve_cna_number",
                        columns: x => new { x.cve_year, x.cve_cna_number },
                        principalTable: "cve",
                        principalColumns: new[] { "year", "cna_number" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vulnerability_point_platform_platform_id",
                        column: x => x.platform_id,
                        principalTable: "platform",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_vulnerability_point_software_software_id",
                        column: x => x.software_id,
                        principalTable: "software",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "cve_solution",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    info = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    solution_link = table.Column<string>(type: "text", nullable: true),
                    download_link = table.Column<string>(type: "text", nullable: true),
                    additional_link = table.Column<string>(type: "text", nullable: true),
                    vulnerability_point_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cve_solution", x => x.id);
                    table.ForeignKey(
                        name: "FK_cve_solution_vulnerability_point_vulnerability_point_id",
                        column: x => x.vulnerability_point_id,
                        principalTable: "vulnerability_point",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cve_solution_vulnerability_point_id",
                table: "cve_solution",
                column: "vulnerability_point_id");

            migrationBuilder.CreateIndex(
                name: "IX_report_request_cve_report_request_id",
                table: "report_request_cve",
                column: "report_request_id");

            migrationBuilder.CreateIndex(
                name: "IX_vulnerability_point_cve_year_cve_cna_number",
                table: "vulnerability_point",
                columns: new[] { "cve_year", "cve_cna_number" });

            migrationBuilder.CreateIndex(
                name: "IX_vulnerability_point_data_source_code",
                table: "vulnerability_point",
                column: "data_source_code");

            migrationBuilder.CreateIndex(
                name: "IX_vulnerability_point_platform_id",
                table: "vulnerability_point",
                column: "platform_id");

            migrationBuilder.CreateIndex(
                name: "IX_vulnerability_point_software_id",
                table: "vulnerability_point",
                column: "software_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cve_resolve_queue");

            migrationBuilder.DropTable(
                name: "cve_solution");

            migrationBuilder.DropTable(
                name: "report_request_cve");

            migrationBuilder.DropTable(
                name: "vulnerability_point");

            migrationBuilder.DropTable(
                name: "report_request");

            migrationBuilder.DropTable(
                name: "cve");

            migrationBuilder.DropTable(
                name: "platform");

            migrationBuilder.DropTable(
                name: "software");
        }
    }
}
