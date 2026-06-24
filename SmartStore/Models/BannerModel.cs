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
    }
}
