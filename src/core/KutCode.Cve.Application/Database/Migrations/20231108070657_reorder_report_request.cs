using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KutCode.Cve.Application.Database.Migrations
{
    /// <inheritdoc />
    public partial class reorder_report_request : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_reorder",
                table: "report_request",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_reorder",
                table: "report_request");
        }
    }
}
