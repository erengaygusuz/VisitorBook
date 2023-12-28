using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisitorBook.Backend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class changing_refresh_token_end_date_to_refresh_token_expiry_time_for_users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RefreshTokenEndDate",
                table: "AspNetUsers",
                newName: "RefreshTokenExpiryTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers",
                newName: "RefreshTokenEndDate");
        }
    }
}
