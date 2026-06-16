using Microsoft.AspNetCore.Mvc;
using SmartStore.Repositories;

namespace SmartStore.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Brand> _brandRepository;
        private readonly IRepository<Category> _categoryRepository;
        public HomeController(IRepository<Product> productRepository,
            IRepository<Brand> brandRepository,
            IRepository<Category> categoryRepository)
        {
            _productRepository = productRepository;
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Store()
        {
            var product = await _productRepository.GetAsync(include: [e => e.Brand!, e => e.Category!]);
            return View(product);
        }
    }
}
