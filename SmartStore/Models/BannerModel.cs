namespace SmartStore.Models
{
    public class BannerModel
    {
        public int Id { get; set; }

        // Slide 1
        public string? Slide1ImageUrl { get; set; } = string.Empty;
        public string? Slide1LinkUrl { get; set; } = string.Empty;

        // Slide 2
        public string? Slide2ImageUrl { get; set; } = string.Empty;
        public string? Slide2LinkUrl { get; set; } = string.Empty;

        // Slide 3
        public string? Slide3ImageUrl { get; set; } = string.Empty;
        public string? Slide3LinkUrl { get; set; } = string.Empty;

        // Slide 4
        public string? Slide4ImageUrl { get; set; } = string.Empty;
        public string? Slide4LinkUrl { get; set; } = string.Empty;

        // Slide 5
        public string? Slide5ImageUrl { get; set; } = string.Empty;
        public string? Slide5LinkUrl { get; set; } = string.Empty;

        // Huawei Offers Banners
        public string? HuaweiTitle { get; set; } = string.Empty;
        public string? HuaweiSubtitle { get; set; } = string.Empty;
        public string? HuaweiLinkUrl { get; set; } = string.Empty;
        public string? HuaweiImageUrl { get; set; } = string.Empty;

        public string? AnkerTitle { get; set; } = string.Empty;
        public string? AnkerSubtitle { get; set; } = string.Empty;
        public string? AnkerLinkUrl { get; set; } = string.Empty;
        public string? AnkerImageUrl { get; set; } = string.Empty;

        // Installment Banners
        public string? Installment1Title { get; set; } = string.Empty;
        public string? Installment1Subtitle { get; set; } = string.Empty;
        public string? Installment1Duration { get; set; } = string.Empty;
        public string? Installment1LinkUrl { get; set; } = string.Empty;

        public string? Installment2Title { get; set; } = string.Empty;
        public string? Installment2Subtitle { get; set; } = string.Empty;
        public string? Installment2Duration { get; set; } = string.Empty;
        public string? Installment2LinkUrl { get; set; } = string.Empty;

        // Grid Banners (Homepage Promo Grid)
        public string? Grid1Title { get; set; } = string.Empty;
        public string? Grid1Subtitle { get; set; } = string.Empty;
        public string? Grid1ImageUrl { get; set; } = string.Empty;
        public string? Grid1LinkUrl { get; set; } = string.Empty;

        public string? Grid2Title { get; set; } = string.Empty;
        public string? Grid2Subtitle { get; set; } = string.Empty;
        public string? Grid2ImageUrl { get; set; } = string.Empty;
        public string? Grid2LinkUrl { get; set; } = string.Empty;

        public string? Grid3Title { get; set; } = string.Empty;
        public string? Grid3Subtitle { get; set; } = string.Empty;
        public string? Grid3ImageUrl { get; set; } = string.Empty;
        public string? Grid3LinkUrl { get; set; } = string.Empty;

        public string? Grid4Title { get; set; } = string.Empty;
        public string? Grid4Subtitle { get; set; } = string.Empty;
        public string? Grid4ImageUrl { get; set; } = string.Empty;
        public string? Grid4LinkUrl { get; set; } = string.Empty;

        public string? Grid5Title { get; set; } = string.Empty;
        public string? Grid5Subtitle { get; set; } = string.Empty;
        public string? Grid5ImageUrl { get; set; } = string.Empty;
        public string? Grid5LinkUrl { get; set; } = string.Empty;

        // Grid Banners Category Links
        public string? Grid1CategoryName { get; set; } = string.Empty;
        public string? Grid2CategoryName { get; set; } = string.Empty;
        public string? Grid3CategoryName { get; set; } = string.Empty;
        public string? Grid4CategoryName { get; set; } = string.Empty;
        public string? Grid5CategoryName { get; set; } = string.Empty;
    }
}
