using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartStore.Migrations
{
    /// <inheritdoc />
    public partial class CleanBannerModelSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnkerImageUrl",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "AnkerLinkUrl",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "AnkerSubtitle",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "AnkerTitle",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "HeroImageUrl",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "HeroLinkUrl",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "HeroSubtitle",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "HeroTitle",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "HuaweiImageUrl",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "HuaweiLinkUrl",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "HuaweiSubtitle",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "HuaweiTitle",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Installment1Duration",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Installment1LinkUrl",
                table: "Banners");

            migrationBuilder.RenameColumn(
                name: "Installment2Title",
                table: "Banners",
                newName: "Slide3LinkUrl");

            migrationBuilder.RenameColumn(
                name: "Installment2Subtitle",
                table: "Banners",
                newName: "Slide3ImageUrl");

            migrationBuilder.RenameColumn(
                name: "Installment2LinkUrl",
                table: "Banners",
                newName: "Slide2LinkUrl");

            migrationBuilder.RenameColumn(
                name: "Installment2Duration",
                table: "Banners",
                newName: "Slide2ImageUrl");

            migrationBuilder.RenameColumn(
                name: "Installment1Title",
                table: "Banners",
                newName: "Slide1LinkUrl");

            migrationBuilder.RenameColumn(
                name: "Installment1Subtitle",
                table: "Banners",
                newName: "Slide1ImageUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Slide3LinkUrl",
                table: "Banners",
                newName: "Installment2Title");

            migrationBuilder.RenameColumn(
                name: "Slide3ImageUrl",
                table: "Banners",
                newName: "Installment2Subtitle");

            migrationBuilder.RenameColumn(
                name: "Slide2LinkUrl",
                table: "Banners",
                newName: "Installment2LinkUrl");

            migrationBuilder.RenameColumn(
                name: "Slide2ImageUrl",
                table: "Banners",
                newName: "Installment2Duration");

            migrationBuilder.RenameColumn(
                name: "Slide1LinkUrl",
                table: "Banners",
                newName: "Installment1Title");

            migrationBuilder.RenameColumn(
                name: "Slide1ImageUrl",
                table: "Banners",
                newName: "Installment1Subtitle");

            migrationBuilder.AddColumn<string>(
                name: "AnkerImageUrl",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AnkerLinkUrl",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AnkerSubtitle",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AnkerTitle",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HeroImageUrl",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HeroLinkUrl",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HeroSubtitle",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HeroTitle",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HuaweiImageUrl",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HuaweiLinkUrl",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HuaweiSubtitle",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HuaweiTitle",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Installment1Duration",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Installment1LinkUrl",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
