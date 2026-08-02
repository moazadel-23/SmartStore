using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using SmartStore.Models;
using SmartStore.Repositories;

namespace SmartStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly IRepository<Brand> _brandRepository;
        private readonly IStringLocalizer<LocalizationController> _localizer;
        public BrandController(IRepository<Brand> brandRepository, IStringLocalizer<LocalizationController> localizer)
        {
            _brandRepository = brandRepository;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var brands = await _brandRepository.GetAsync();
            return View(brands);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand brand, CancellationToken cancellationToken)
        {
            if(ModelState.IsValid)
            {
                await _brandRepository.AddAsync(brand, cancellationToken);
                await _brandRepository.Commit(cancellationToken);
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var brand = await _brandRepository.GetOneAsync(e => e.Id == id);
            if (brand == null)
                return NotFound();
                
            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Brand brand)
        {
            if (ModelState.IsValid)
            {
                _brandRepository.Update(brand);
                await _brandRepository.Commit();
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _brandRepository.GetOneAsync(e => e.Id == id);
            if (brand is null) return NotFound();
            
            try
            {
                _brandRepository.Delete(brand);
                await _brandRepository.Commit();
                TempData["SuccessMessage"] = _localizer["BrandDeletedSuccessfully"].Value;
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = _localizer["BrandDeleteFailed"].Value;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {
            var brand = await _brandRepository.GetOneAsync(e => e.Id == id, cancellationToken: cancellationToken);
            if (brand is null) return NotFound(); 
            
            try
            {
                _brandRepository.Delete(brand);
                await _brandRepository.Commit(cancellationToken);
                TempData["SuccessMessage"] = _localizer["BrandDeletedSuccessfully"].Value;
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = _localizer["BrandDeleteFailed"].Value;
            }
            return RedirectToAction(nameof(Index));
        }    
    }
}
