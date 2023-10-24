using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KutCode.Cve.Application.Database.Migrations
{
    /// <inheritdoc />
    public partial class cve_resolve_queue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(name: "cve_finder_queue", newName: "cve_resolve_queue");
            migrationBuilder.RenameColumn(
                name: "finder_code",
                table: "cve_resolve_queue",
                newName: "resolver_code");
            migrationBuilder.AddColumn<bool>(name: "update_cve", table: "cve_resolve_queue", type: "boolean", nullable: false, defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(name: "cve_resolve_queue", newName: "cve_finder_queue");
        
            migrationBuilder.RenameColumn(
                name: "resolver_code",
                table: "cve_resolve_queue",
                newName: "finder_code");
            
            migrationBuilder.DropColumn(name: "update_cve", table: "cve_resolve_queue");
        }
    }
}
