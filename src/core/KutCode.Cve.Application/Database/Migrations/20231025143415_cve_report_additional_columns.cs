using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KutCode.Cve.Application.Database.Migrations
{
    /// <inheritdoc />
    public partial class cve_report_additional_columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "sys_created",
                table: "report_request",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sys_created",
                table: "report_request");
        }
    }
}
