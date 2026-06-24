namespace SmartStore.ViewModel.Store
{
    public class ProductDetailViewModel
    {
        public Product Product { get; set; } = null!;
        public IEnumerable<Product> RelatedProducts { get; set; } = Enumerable.Empty<Product>();
        public IEnumerable<Product> FrequentlyBoughtTogether { get; set; } = Enumerable.Empty<Product>();
        public string? BreadcrumbCategory { get; set; }
        public string? BreadcrumbSubcategory { get; set; }
    }
}
