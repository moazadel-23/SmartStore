using Microsoft.AspNetCore.Mvc;
using SmartStore.Models;
using SmartStore.Repositories;

namespace SmartStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PromotionController : Controller
    {
        private readonly IRepository<Promotion> _promotionRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Brand> _brandRepository;

        public PromotionController(
            IRepository<Promotion> promotionRepository,
            IRepository<Product> productRepository,
            IRepository<Category> categoryRepository,
            IRepository<Brand> brandRepository)
        {
            _promotionRepository = promotionRepository;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var promotions = await _promotionRepository.GetAsync(
                include: [p => p.Product!, p => p.Category!, p => p.Brand!],
                cancellationToken: cancellationToken);
            return View(promotions);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            ViewBag.products = await _productRepository.GetAsync(cancellationToken: cancellationToken) ?? new List<Product>();
            ViewBag.categories = await _categoryRepository.GetAsync(cancellationToken: cancellationToken) ?? new List<Category>();
            ViewBag.brands = await _brandRepository.GetAsync(cancellationToken: cancellationToken) ?? new List<Brand>();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Promotion promotion, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                await _promotionRepository.AddAsync(promotion, cancellationToken);
                await _promotionRepository.Commit(cancellationToken);
                TempData["SuccessMessage"] = "تم إضافة العرض الترويجي بنجاح!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.products = await _productRepository.GetAsync(cancellationToken: cancellationToken) ?? new List<Product>();
            ViewBag.categories = await _categoryRepository.GetAsync(cancellationToken: cancellationToken) ?? new List<Category>();
            ViewBag.brands = await _brandRepository.GetAsync(cancellationToken: cancellationToken) ?? new List<Brand>();
            return View(promotion);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id, CancellationToken cancellationToken)
        {
            if (id == null || id == 0)
                return NotFound();

            var promotion = await _promotionRepository.GetOneAsync(e => e.Id == id, cancellationToken: cancellationToken);
            if (promotion == null)
                return NotFound();

            ViewBag.products = await _productRepository.GetAsync(cancellationToken: cancellationToken) ?? new List<Product>();
            ViewBag.categories = await _categoryRepository.GetAsync(cancellationToken: cancellationToken) ?? new List<Category>();
            ViewBag.brands = await _brandRepository.GetAsync(cancellationToken: cancellationToken) ?? new List<Brand>();
            return View(promotion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Promotion promotion, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                _promotionRepository.Update(promotion);
                await _promotionRepository.Commit(cancellationToken);
                TempData["SuccessMessage"] = "تم تعديل العرض الترويجي بنجاح!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.products = await _productRepository.GetAsync(cancellationToken: cancellationToken) ?? new List<Product>();
            ViewBag.categories = await _categoryRepository.GetAsync(cancellationToken: cancellationToken) ?? new List<Category>();
            ViewBag.brands = await _brandRepository.GetAsync(cancellationToken: cancellationToken) ?? new List<Brand>();
            return View(promotion);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var promotion = await _promotionRepository.GetOneAsync(e => e.Id == id, cancellationToken: cancellationToken);
            if (promotion is null) return NotFound();
            
            _promotionRepository.Delete(promotion);
            await _promotionRepository.Commit(cancellationToken);
            TempData["SuccessMessage"] = "تم حذف العرض الترويجي بنجاح!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {
            var promotion = await _promotionRepository.GetOneAsync(e => e.Id == id, cancellationToken: cancellationToken);
            if (promotion is null) return NotFound(); 
            
            _promotionRepository.Delete(promotion);
            await _promotionRepository.Commit(cancellationToken);
            TempData["SuccessMessage"] = "تم حذف العرض الترويجي بنجاح!";
            return RedirectToAction(nameof(Index));
        }
    }
}
