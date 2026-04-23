using Microsoft.AspNetCore.Mvc;
using SmartStore.Repositories;

namespace SmartStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IRepository<Category> _categoryRepository;
        public CategoryController(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
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
        public async Task<IActionResult> Create(Category category, CancellationToken cancellationToken)
        {
            if(ModelState.IsValid)
            {
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
        public async Task<IActionResult> Update(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRepository.Update(category);
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
            
            _categoryRepository.Delete(category);
            await _categoryRepository.Commit();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetOneAsync(e => e.Id == id);
            if (category is null) return NotFound(); 
            
            _categoryRepository.Delete(category);
            await _categoryRepository.Commit(cancellationToken);
            return RedirectToAction(nameof(Index));
        }    
    }
}
