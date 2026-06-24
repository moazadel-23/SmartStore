using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartStore.Migrations
{
    /// <inheritdoc />
    public partial class AddAnkerImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnkerImageUrl",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnkerImageUrl",
                table: "Banners");
        }
    }
}
