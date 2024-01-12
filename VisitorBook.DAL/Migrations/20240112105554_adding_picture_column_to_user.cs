using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisitorBook.DAL.Migrations
{
    /// <inheritdoc />
    public partial class adding_picture_column_to_user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "AspNetUsers");
        }
    }
}
