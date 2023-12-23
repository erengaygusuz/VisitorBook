using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisitorBook.Backend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class deleting_gid_from_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GId",
                table: "Visitors");

            migrationBuilder.DropColumn(
                name: "GId",
                table: "VisitorAddress");

            migrationBuilder.DropColumn(
                name: "GId",
                table: "VisitedCounties");

            migrationBuilder.DropColumn(
                name: "GId",
                table: "SubRegions");

            migrationBuilder.DropColumn(
                name: "GId",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "GId",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "GId",
                table: "Counties");

            migrationBuilder.DropColumn(
                name: "GId",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "GId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GId",
                table: "AspNetRoles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GId",
                table: "Visitors",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "GId",
                table: "VisitorAddress",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "GId",
                table: "VisitedCounties",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "GId",
                table: "SubRegions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "GId",
                table: "Regions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "GId",
                table: "Countries",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "GId",
                table: "Counties",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "GId",
                table: "Cities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "GId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "GId",
                table: "AspNetRoles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
