using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisitorBook.DAL.Migrations
{
    /// <inheritdoc />
    public partial class inheriting_audit_trail_from_base_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "AuditTrails",
                newName: "CreatedDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "AuditTrails",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "AuditTrails");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "AuditTrails",
                newName: "TimeStamp");
        }
    }
}
