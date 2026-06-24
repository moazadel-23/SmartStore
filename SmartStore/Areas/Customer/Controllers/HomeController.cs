using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartStore.Repositories;
using SmartStore.ViewModel.Store;

namespace SmartStore.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Brand> _brandRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Favorite> _favoriteRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(IRepository<Product> productRepository,
            IRepository<Brand> brandRepository,
            IRepository<Category> categoryRepository,
            IRepository<Favorite> favoriteRepository,
            UserManager<ApplicationUser> userManager)
        {
            _productRepository = productRepository;
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
            _favoriteRepository = favoriteRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var all = await _productRepository.GetAsync(
                include: [e => e.Brand!, e => e.Category!, e => e.ProductSubImgs],
                tracked: false);

            var vm = new StoreViewModel
            {
                Products = all.OrderByDescending(p => p.Rate).ToList(),
                BestSellers = all.OrderByDescending(p => p.Rate).Take(8).ToList(),
                NewArrivals = all.OrderByDescending(p => p.CreateAt).Take(8).ToList(),
                Categories = await _categoryRepository.GetAsync(tracked: false),
                Brands = await _brandRepository.GetAsync(tracked: false)
            };

            await LoadWishlistAsync();

            return View(vm);
        }

        public async Task<IActionResult> Store(StoreViewModel vm, bool partial = false)
        {
            var allRaw = await _productRepository.GetAsync(
                include: [e => e.Brand!, e => e.Category!, e => e.ProductSubImgs],
                tracked: false);

            var f = vm.Filter ?? new FilterState();

            var filtered = allRaw.AsQueryable();

            if (f.CategoryIds?.Any() == true)
                filtered = filtered.Where(p => f.CategoryIds.Contains(p.CategoryId));
            if (f.BrandIds?.Any() == true)
                filtered = filtered.Where(p => f.BrandIds.Contains(p.BrandId));
            if (f.MinPrice.HasValue)
                filtered = filtered.Where(p => p.Price >= f.MinPrice.Value);
            if (f.MaxPrice.HasValue)
                filtered = filtered.Where(p => p.Price <= f.MaxPrice.Value);
            if (f.MinRating.HasValue)
                filtered = filtered.Where(p => p.Rate >= f.MinRating.Value);
            if (f.InStockOnly == true)
                filtered = filtered.Where(p => p.Quantity > 0);
            if (!string.IsNullOrWhiteSpace(vm.SearchQuery))
                filtered = filtered.Where(p =>
                    p.Name.Contains(vm.SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                    p.description.Contains(vm.SearchQuery, StringComparison.OrdinalIgnoreCase));

            filtered = f.SortBy switch
            {
                "price-low" => filtered.OrderBy(p => p.Price),
                "price-high" => filtered.OrderByDescending(p => p.Price),
                "newest" => filtered.OrderByDescending(p => p.CreateAt),
                "rating" => filtered.OrderByDescending(p => p.Rate),
                _ => filtered.OrderByDescending(p => p.Rate)
            };

            var total = filtered.Count();
            var page = vm.Pagination?.Page ?? 1;
            var pageSize = vm.Pagination?.PageSize ?? 12;
            var paged = filtered.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var result = new StoreViewModel
            {
                Products = paged,
                BestSellers = allRaw.OrderByDescending(p => p.Rate).Take(8).ToList(),
                NewArrivals = allRaw.OrderByDescending(p => p.CreateAt).Take(8).ToList(),
                Categories = await _categoryRepository.GetAsync(tracked: false),
                Brands = await _brandRepository.GetAsync(tracked: false),
                Filter = f,
                Pagination = new PaginationState
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = total
                },
                SearchQuery = vm.SearchQuery
            };

            await LoadWishlistAsync();

            if (partial)
                return PartialView("_ProductGrid", result);

            return View(result);
        }

      

        public async Task<IActionResult> Product(int id)
        {
            var product = await _productRepository.GetOneAsync(
                expression: p => p.Id == id,
                include: [e => e.Brand!, e => e.Category!, e => e.ProductSubImgs],
                tracked: false);

            if (product == null) return RedirectToAction("Store");

            var allProducts = await _productRepository.GetAsync(
                include: [e => e.Brand!, e => e.Category!, e => e.ProductSubImgs],
                tracked: false);

            var related = allProducts
                .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id)
                .OrderByDescending(p => p.Rate)
                .Take(4);

            var boughtTogether = allProducts
                .Where(p => p.Id != product.Id)
                .OrderByDescending(p => p.Rate)
                .Take(3);

            var vm = new ProductDetailViewModel
            {
                Product = product,
                RelatedProducts = related,
                FrequentlyBoughtTogether = boughtTogether,
                BreadcrumbCategory = product.Category?.Name,
                BreadcrumbSubcategory = product.Brand?.Name
            };

            await LoadWishlistAsync();

            return View(vm);
        }

        private async Task LoadWishlistAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var favs = await _favoriteRepository.GetAsync(
                    e => e.UserId == user.Id,
                    include: [e => e.Product!],
                    tracked: false);
                ViewBag.WishlistProducts = favs.Where(f => f.Product != null).Select(f => f.Product!).ToList();
            }
            else
            {
                ViewBag.WishlistProducts = new List<Product>();
            }
        }
    }
}
