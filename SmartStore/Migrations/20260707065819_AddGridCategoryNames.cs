using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartStore.Migrations
{
    /// <inheritdoc />
    public partial class AddGridCategoryNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Grid1CategoryName",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grid2CategoryName",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grid3CategoryName",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grid4CategoryName",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grid5CategoryName",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grid1CategoryName",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Grid2CategoryName",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Grid3CategoryName",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Grid4CategoryName",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Grid5CategoryName",
                table: "Banners");
        }
    }
}
