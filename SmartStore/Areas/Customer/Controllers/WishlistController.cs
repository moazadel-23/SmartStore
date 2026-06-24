using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartStore.Repositories;

namespace SmartStore.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class WishlistController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Favorite> _favoriteRepository;
        private readonly IRepository<Product> _productRepository;

        public WishlistController(
            UserManager<ApplicationUser> userManager,
            IRepository<Favorite> favoriteRepository,
            IRepository<Product> productRepository)
        {
            _userManager = userManager;
            _favoriteRepository = favoriteRepository;
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index(string searchQuery = null!)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var favorites = await _favoriteRepository.GetAsync(
                e => e.UserId == user.Id,
                include: [e => e.Product!]);

            var products = favorites.Where(e => e.Product != null).Select(e => e.Product!);

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                products = products.Where(p => 
                    p.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) || 
                    (p.description != null && p.description.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)));
            }

            return View(products.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> AddToFavorites(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var existing = (await _favoriteRepository.GetAsync(
                e => e.UserId == user.Id && e.ProductId == productId)).FirstOrDefault();

            if (existing != null)
            {
                _favoriteRepository.Delete(existing);
            }
            else
            {
                var product = await _productRepository.GetOneAsync(e => e.Id == productId, tracked: false);
                if (product == null) return NotFound();

                await _favoriteRepository.AddAsync(new Favorite
                {
                    UserId = user.Id,
                    ProductId = productId
                });
            }

            await _favoriteRepository.Commit();

            var count = (await _favoriteRepository.GetAsync(e => e.UserId == user.Id, tracked: false)).Count();
            return Json(new { success = true, added = existing == null, count });
        }
        [HttpPost]
        public async Task<IActionResult> RemoveProduct(int productId, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return NotFound();

            var favorite = await _favoriteRepository.GetOneAsync(
                e => e.ProductId == productId && e.UserId == user.Id);

            if (favorite != null)
            {
                _favoriteRepository.Delete(favorite);
                await _favoriteRepository.Commit(cancellationToken);
                TempData["Message"] = "Product removed successfully.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
