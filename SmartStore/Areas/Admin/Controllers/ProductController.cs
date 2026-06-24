using Microsoft.AspNetCore.Mvc;
using SmartStore.Models;
using SmartStore.Repositories;
using System.Linq.Expressions;

namespace SmartStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Brand> _brandRepository;

        public ProductController(
            IRepository<Product> productRepository,
            IRepository<Category> categoryRepository,
            IRepository<Brand> brandRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAsync(include: [e => e.Category!, e => e.Brand!]);
            return View(products);
        }
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            ViewBag.category = await _categoryRepository.GetAsync(cancellationToken: cancellationToken) ?? new List<Category>();
            ViewBag.brand = await _brandRepository.GetAsync(cancellationToken: cancellationToken) ?? new List<Brand>();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(
            Product product,
            IFormFile Img,
            List<IFormFile> SubImgFiles,
            int CategoryId,
            int BrandId,
            CancellationToken cancellationToken)
        {
           if(!ModelState.IsValid)
           {
              ViewBag.category= await _categoryRepository.GetAsync(cancellationToken:cancellationToken)?? new List<Category>();
              ViewBag.brand= await _brandRepository.GetAsync(cancellationToken: cancellationToken) ?? new List<Brand>();
              return View(product);
           }
            product.CategoryId = CategoryId;
            product.BrandId = BrandId;

            if (Img != null && Img.Length > 0)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\ProductImg");
                Directory.CreateDirectory(folder);
                var fileName = Guid.NewGuid() + Path.GetExtension(Img.FileName);
                var filePath = Path.Combine(folder, fileName);
                using var stream = System.IO.File.Create(filePath);
                await Img.CopyToAsync(stream);
                product.MainImg = fileName;
            }

            if (SubImgFiles != null && SubImgFiles.Count > 0)
            {
                if (product.ProductSubImgs == null)
                    product.ProductSubImgs = new List<ProductImges>();

                foreach (var file in SubImgFiles)
                {
                    var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\ProductSubImges");
                    Directory.CreateDirectory(folder);
                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(folder, fileName);
                    using var stream = System.IO.File.Create(filePath);
                    await file.CopyToAsync(stream);
                    product.ProductSubImgs.Add(new ProductImges
                    {
                        SubImg = fileName
                    });
                }
            }
            await _productRepository.AddAsync(product, cancellationToken);
            await _productRepository.Commit(cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetOneAsync(e => e.Id == id, include: [e => e.Category!, e => e.Brand!, e => e.ProductSubImgs!], tracked: true);
            if (product == null) return NotFound();

            ViewBag.category = await _categoryRepository.GetAsync(cancellationToken: cancellationToken) ?? new List<Category>();
            ViewBag.brand = await _brandRepository.GetAsync(cancellationToken: cancellationToken) ?? new List<Brand>();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(
            Product model,
            IFormFile? Img,
            List<IFormFile>? SubImgFiles,
            List<int>? DeletedSubImgIds,
            int CategoryId,
            int BrandId,
            CancellationToken cancellationToken)
        {
            var existingProduct = await _productRepository.GetOneAsync(e => e.Id == model.Id, include: [e => e.ProductSubImgs!], tracked: true);
            if (existingProduct == null) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.category = await _categoryRepository.GetAsync(cancellationToken: cancellationToken) ?? new List<Category>();
                ViewBag.brand = await _brandRepository.GetAsync(cancellationToken: cancellationToken) ?? new List<Brand>();
                return View(model);
            }

            existingProduct.Name = model.Name;
            existingProduct.description = model.description;
            existingProduct.Price = model.Price;
            existingProduct.Quantity = model.Quantity;
            existingProduct.Discount = model.Discount;
            existingProduct.Rate = model.Rate;
            existingProduct.Status = model.Status;
            existingProduct.CategoryId = CategoryId;
            existingProduct.BrandId = BrandId;

            if (Img != null && Img.Length > 0)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\ProductImg");
                Directory.CreateDirectory(folder);
                var fileName = Guid.NewGuid() + Path.GetExtension(Img.FileName);
                var filePath = Path.Combine(folder, fileName);
                using var stream = System.IO.File.Create(filePath);
                await Img.CopyToAsync(stream);
                existingProduct.MainImg = fileName;
            }

            if (SubImgFiles != null && SubImgFiles.Count > 0)
            {
                if (existingProduct.ProductSubImgs == null)
                    existingProduct.ProductSubImgs = new List<ProductImges>();

                foreach (var file in SubImgFiles)
                {
                    var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\ProductSubImges");
                    Directory.CreateDirectory(folder);
                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(folder, fileName);
                    using var stream = System.IO.File.Create(filePath);
                    await file.CopyToAsync(stream);
                    existingProduct.ProductSubImgs.Add(new ProductImges
                    {
                        SubImg = fileName
                    });
                }
            }

            if (DeletedSubImgIds != null && DeletedSubImgIds.Count > 0 && existingProduct.ProductSubImgs != null)
            {
                var imgsToRemove = existingProduct.ProductSubImgs.Where(img => DeletedSubImgIds.Contains(img.Id)).ToList();
                foreach (var imgToRemove in imgsToRemove)
                {
                    existingProduct.ProductSubImgs.Remove(imgToRemove);
                }
            }

            await _productRepository.Commit(cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetOneAsync(e => e.Id == id);
            if (product is null) return NotFound();
            
            _productRepository.Delete(product);
            await _productRepository.Commit();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetOneAsync(e => e.Id == id);
            if (product is null) return NotFound(); 
            
            _productRepository.Delete(product);
            await _productRepository.Commit(cancellationToken);
            return RedirectToAction(nameof(Index));
        }
    }
}
