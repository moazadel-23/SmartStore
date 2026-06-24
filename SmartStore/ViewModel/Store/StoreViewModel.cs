namespace SmartStore.ViewModel.Store
{
    public class StoreViewModel
    {
        public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
        public IEnumerable<Product> BestSellers { get; set; } = Enumerable.Empty<Product>();
        public IEnumerable<Product> NewArrivals { get; set; } = Enumerable.Empty<Product>();
        public IEnumerable<Category> Categories { get; set; } = Enumerable.Empty<Category>();
        public IEnumerable<Brand> Brands { get; set; } = Enumerable.Empty<Brand>();

        public FilterState Filter { get; set; } = new();
        public PaginationState Pagination { get; set; } = new();
        public string? SearchQuery { get; set; }
    }

    public class FilterState
    {
        public List<int> CategoryIds { get; set; } = new();
        public List<int> BrandIds { get; set; } = new();
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinRating { get; set; }
        public bool? InStockOnly { get; set; }
        public string SortBy { get; set; } = "popular";
    }

    public class PaginationState
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPrevious => Page > 1;
        public bool HasNext => Page < TotalPages;
    }
}
