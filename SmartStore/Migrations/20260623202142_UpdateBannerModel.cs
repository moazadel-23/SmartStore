using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartStore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBannerModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slide1LinkUrl",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Slide2LinkUrl",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Slide3LinkUrl",
                table: "Banners");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slide1LinkUrl",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Slide2LinkUrl",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Slide3LinkUrl",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
