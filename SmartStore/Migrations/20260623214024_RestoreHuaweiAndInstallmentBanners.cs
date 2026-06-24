using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartStore.Migrations
{
    /// <inheritdoc />
    public partial class RestoreHuaweiAndInstallmentBanners : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnkerImageUrl",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AnkerLinkUrl",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AnkerSubtitle",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AnkerTitle",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HuaweiImageUrl",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HuaweiLinkUrl",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HuaweiSubtitle",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HuaweiTitle",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Installment1Duration",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Installment1LinkUrl",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Installment1Subtitle",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Installment1Title",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Installment2Duration",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Installment2LinkUrl",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Installment2Subtitle",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Installment2Title",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Installment1Subtitle",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Installment1Title",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Installment2Duration",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Installment2LinkUrl",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Installment2Subtitle",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Installment2Title",
                table: "Banners");
        }
    }
}
