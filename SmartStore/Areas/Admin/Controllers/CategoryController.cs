using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using SmartStore.Repositories;

namespace SmartStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IStringLocalizer<LocalizationController> _localizer;
        public CategoryController(IRepository<Category> categoryRepository, IStringLocalizer<LocalizationController> localizer)
        {
            _categoryRepository = categoryRepository;
            _localizer = localizer;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category, IFormFile? ImageFile, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img");
                    Directory.CreateDirectory(folder);
                    var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                    var filePath = Path.Combine(folder, fileName);
                    using var stream = System.IO.File.Create(filePath);
                    await ImageFile.CopyToAsync(stream);
                    category.ImageUrl = "/img/" + fileName;
                }

                await _categoryRepository.AddAsync(category, cancellationToken);
                await _categoryRepository.Commit(cancellationToken);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var category = await _categoryRepository.GetOneAsync(e => e.Id == id);
            if (category == null)
                return NotFound();
                
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Category category, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                var existingCategory = await _categoryRepository.GetOneAsync(e => e.Id == category.Id, tracked: true);
                if (existingCategory == null)
                    return NotFound();

                existingCategory.Name = category.Name;
                existingCategory.Description = category.Description;
                existingCategory.Status = category.Status;

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img");
                    Directory.CreateDirectory(folder);
                    var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                    var filePath = Path.Combine(folder, fileName);
                    using var stream = System.IO.File.Create(filePath);
                    await ImageFile.CopyToAsync(stream);
                    existingCategory.ImageUrl = "/img/" + fileName;
                }

                _categoryRepository.Update(existingCategory);
                await _categoryRepository.Commit();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepository.GetOneAsync(e => e.Id == id);
            if (category is null) return NotFound();
            
            try
            {
                _categoryRepository.Delete(category);
                await _categoryRepository.Commit();
                TempData["SuccessMessage"] = _localizer["CategoryDeletedSuccessfully"].Value;
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = _localizer["CategoryDeleteFailed"].Value;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetOneAsync(e => e.Id == id, cancellationToken: cancellationToken);
            if (category is null) return NotFound(); 
            
            try
            {
                _categoryRepository.Delete(category);
                await _categoryRepository.Commit(cancellationToken);
                TempData["SuccessMessage"] = _localizer["CategoryDeletedSuccessfully"].Value;
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = _localizer["CategoryDeleteFailed"].Value;
            }
            return RedirectToAction(nameof(Index));
        }    
    }
}
