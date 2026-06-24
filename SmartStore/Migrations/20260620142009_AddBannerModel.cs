using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartStore.Migrations
{
    /// <inheritdoc />
    public partial class AddBannerModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Categories");

            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeroTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeroSubtitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeroImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeroLinkUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Installment1Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Installment1Subtitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Installment1Duration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Installment1LinkUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Installment2Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Installment2Subtitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Installment2Duration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Installment2LinkUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HuaweiTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HuaweiSubtitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HuaweiLinkUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HuaweiImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnkerTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnkerSubtitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnkerLinkUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banners");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
