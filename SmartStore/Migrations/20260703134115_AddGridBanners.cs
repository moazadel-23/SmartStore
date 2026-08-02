using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartStore.Migrations
{
    /// <inheritdoc />
    public partial class AddGridBanners : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Grid1ImageUrl",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grid1LinkUrl",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grid1Subtitle",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grid1Title",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grid2ImageUrl",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grid2LinkUrl",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grid2Subtitle",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grid2Title",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grid3ImageUrl",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grid3LinkUrl",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grid3Subtitle",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grid3Title",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grid4ImageUrl",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grid4LinkUrl",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grid4Subtitle",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grid4Title",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grid5ImageUrl",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grid5LinkUrl",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grid5Subtitle",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grid5Title",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grid1ImageUrl",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Grid1LinkUrl",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Grid1Subtitle",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Grid1Title",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Grid2ImageUrl",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Grid2LinkUrl",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Grid2Subtitle",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Grid2Title",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Grid3ImageUrl",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Grid3LinkUrl",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Grid3Subtitle",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Grid3Title",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Grid4ImageUrl",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Grid4LinkUrl",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Grid4Subtitle",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Grid4Title",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Grid5ImageUrl",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Grid5LinkUrl",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Grid5Subtitle",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Grid5Title",
                table: "Banners");
        }
    }
}
