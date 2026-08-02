using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SmartStore.DataAccess;
using SmartStore.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SmartStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BannerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<LocalizationController> _localizer;

        public BannerController(ApplicationDbContext context, IStringLocalizer<LocalizationController> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        private BannerModel GetDefaultBanners()
        {
            return new BannerModel
            {
                Slide1ImageUrl = "/Banners/banner_slide1.png",
                Slide1LinkUrl = "/Customer/Home/Store",
                Slide2ImageUrl = "/Banners/banner_slide2.png",
                Slide2LinkUrl = "/Customer/Home/Store",
                Slide3ImageUrl = "/Banners/banner_slide3.png",
                Slide3LinkUrl = "/Customer/Home/Store",
                Slide4ImageUrl = "/Banners/banner_slide4.png",
                Slide4LinkUrl = "/Customer/Home/Store",
                Slide5ImageUrl = "/Banners/banner_slide5.png",
                Slide5LinkUrl = "/Customer/Home/Store",

                HuaweiTitle = "مهرجان هواوي HUAWEI",
                HuaweiSubtitle = "احصل على كوبونات خصم فورية وهدايا مجانية عند شراء أي من أجهزة هواوي لابتوب أو ساعة ذكية.",
                HuaweiLinkUrl = "/Customer/Home/Store?SearchQuery=Huawei",
                HuaweiImageUrl = "/Banners/6dd23630-6367-416d-9d5f-8e35a0ad917d.avif",

                AnkerTitle = "أنكر ANKER لشواحن الهواتف وملحقاتها",
                AnkerSubtitle = "أقوى حماية وأسرع شحن لهواتفك الذكية، عروض خصم تصل لـ 25% حصرياً.",
                AnkerLinkUrl = "/Customer/Home/Store?SearchQuery=Anker",
                AnkerImageUrl = "/Banners/285ed515-8497-4161-9279-8372cb39b622.avif",

                Installment1Title = "قسط على سعر الكاش",
                Installment1Subtitle = "0% فوائد | 0% مقدم | 0% مصاريف إدارية",
                Installment1Duration = "12 شهر",
                Installment1LinkUrl = "/Customer/Home/Store",

                Installment2Title = "قسط على سعر الكاش",
                Installment2Subtitle = "0% فوائد | 0% مقدم | 0% مصاريف إدارية",
                Installment2Duration = "3 شهور",
                Installment2LinkUrl = "/Customer/Home/Store",

                // Default Grid Banners
                Grid1Title = "أحدث الموديلات",
                Grid1Subtitle = "بأفضل الأسعار",
                Grid1ImageUrl = "/Banners/promo_laptops.png",
                Grid1LinkUrl = "/Customer/Home/Store?Filter.CategoryIds=2",
                Grid1CategoryName = "Electronics",

                Grid2Title = "كل الأجهزة اللي",
                Grid2Subtitle = "بيتك محتاجها في مكان واحد",
                Grid2ImageUrl = "https://images.unsplash.com/photo-1584622650111-993a426fbf0a?w=600&auto=format&fit=crop&q=60",
                Grid2LinkUrl = "/Customer/Home/Store?Filter.CategoryIds=4",
                Grid2CategoryName = "Home Appliances",

                Grid3Title = "تصميم أنيق",
                Grid3Subtitle = "وأداء يفوق التوقعات",
                Grid3ImageUrl = "https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?w=600&auto=format&fit=crop&q=60",
                Grid3LinkUrl = "/Customer/Home/Store?Filter.CategoryIds=1",
                Grid3CategoryName = "Electronics",

                Grid4Title = "إكسسوارات",
                Grid4Subtitle = "تليق بك",
                Grid4ImageUrl = "https://images.unsplash.com/photo-1608248597279-f99d160bfcbc?w=600&auto=format&fit=crop&q=60",
                Grid4LinkUrl = "/Customer/Home/Store?Filter.CategoryIds=6",
                Grid4CategoryName = "Electronics",

                Grid5Title = "نظافة مثالية",
                Grid5Subtitle = "من غير مجهود",
                Grid5ImageUrl = "https://images.unsplash.com/photo-1584622781564-1d987f7333c1?w=600&auto=format&fit=crop&q=60",
                Grid5LinkUrl = "/Customer/Home/Store?Filter.CategoryIds=4",
                Grid5CategoryName = "Home Appliances"
            };
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            BannerModel model = await _context.Banners.FirstOrDefaultAsync();

            if (model == null)
            {
                model = GetDefaultBanners();
                _context.Banners.Add(model);
                await _context.SaveChangesAsync();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(BannerModel model,
            IFormFile? Slide1ImgFile, IFormFile? Slide2ImgFile, IFormFile? Slide3ImgFile,
            IFormFile? Slide4ImgFile, IFormFile? Slide5ImgFile,
            IFormFile? HuaweiImgFile, IFormFile? AnkerImgFile,
            IFormFile? Grid1ImgFile, IFormFile? Grid2ImgFile, IFormFile? Grid3ImgFile, IFormFile? Grid4ImgFile, IFormFile? Grid5ImgFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingModel = await _context.Banners.FirstOrDefaultAsync();
                    if (existingModel == null)
                    {
                        existingModel = GetDefaultBanners();
                        _context.Banners.Add(existingModel);
                        await _context.SaveChangesAsync();
                    }

                    var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Banners");
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    // Handle Slide 1
                    if (Slide1ImgFile != null && Slide1ImgFile.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(Slide1ImgFile.FileName);
                        var filePath = Path.Combine(folder, fileName);
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await Slide1ImgFile.CopyToAsync(stream);
                        }
                        existingModel.Slide1ImageUrl = "/Banners/" + fileName;
                    }
                    existingModel.Slide1LinkUrl = model.Slide1LinkUrl;

                    // Handle Slide 2
                    if (Slide2ImgFile != null && Slide2ImgFile.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(Slide2ImgFile.FileName);
                        var filePath = Path.Combine(folder, fileName);
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await Slide2ImgFile.CopyToAsync(stream);
                        }
                        existingModel.Slide2ImageUrl = "/Banners/" + fileName;
                    }
                    existingModel.Slide2LinkUrl = model.Slide2LinkUrl;

                    // Handle Slide 3
                    if (Slide3ImgFile != null && Slide3ImgFile.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(Slide3ImgFile.FileName);
                        var filePath = Path.Combine(folder, fileName);
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await Slide3ImgFile.CopyToAsync(stream);
                        }
                        existingModel.Slide3ImageUrl = "/Banners/" + fileName;
                    }
                    existingModel.Slide3LinkUrl = model.Slide3LinkUrl;

                    // Handle Slide 4
                    if (Slide4ImgFile != null && Slide4ImgFile.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(Slide4ImgFile.FileName);
                        var filePath = Path.Combine(folder, fileName);
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await Slide4ImgFile.CopyToAsync(stream);
                        }
                        existingModel.Slide4ImageUrl = "/Banners/" + fileName;
                    }
                    existingModel.Slide4LinkUrl = model.Slide4LinkUrl;

                    // Handle Slide 5
                    if (Slide5ImgFile != null && Slide5ImgFile.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(Slide5ImgFile.FileName);
                        var filePath = Path.Combine(folder, fileName);
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await Slide5ImgFile.CopyToAsync(stream);
                        }
                        existingModel.Slide5ImageUrl = "/Banners/" + fileName;
                    }
                    existingModel.Slide5LinkUrl = model.Slide5LinkUrl;

                    // Handle Huawei Banner
                    existingModel.HuaweiTitle = model.HuaweiTitle;
                    existingModel.HuaweiSubtitle = model.HuaweiSubtitle;
                    existingModel.HuaweiLinkUrl = model.HuaweiLinkUrl;
                    if (HuaweiImgFile != null && HuaweiImgFile.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(HuaweiImgFile.FileName);
                        var filePath = Path.Combine(folder, fileName);
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await HuaweiImgFile.CopyToAsync(stream);
                        }
                        existingModel.HuaweiImageUrl = "/Banners/" + fileName;
                    }

                    // Handle Anker Banner
                    existingModel.AnkerTitle = model.AnkerTitle;
                    existingModel.AnkerSubtitle = model.AnkerSubtitle;
                    existingModel.AnkerLinkUrl = model.AnkerLinkUrl;
                    if (AnkerImgFile != null && AnkerImgFile.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(AnkerImgFile.FileName);
                        var filePath = Path.Combine(folder, fileName);
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await AnkerImgFile.CopyToAsync(stream);
                        }
                        existingModel.AnkerImageUrl = "/Banners/" + fileName;
                    }

                    // Handle Installment 1
                    existingModel.Installment1Title = model.Installment1Title;
                    existingModel.Installment1Subtitle = model.Installment1Subtitle;
                    existingModel.Installment1Duration = model.Installment1Duration;
                    existingModel.Installment1LinkUrl = model.Installment1LinkUrl;

                    // Handle Installment 2
                    existingModel.Installment2Title = model.Installment2Title;
                    existingModel.Installment2Subtitle = model.Installment2Subtitle;
                    existingModel.Installment2Duration = model.Installment2Duration;
                    existingModel.Installment2LinkUrl = model.Installment2LinkUrl;

                    // Handle Grid Banners
                    existingModel.Grid1Title = model.Grid1Title;
                    existingModel.Grid1Subtitle = model.Grid1Subtitle;
                    existingModel.Grid1LinkUrl = model.Grid1LinkUrl;
                    existingModel.Grid1CategoryName = model.Grid1CategoryName;
                    if (Grid1ImgFile != null && Grid1ImgFile.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(Grid1ImgFile.FileName);
                        var filePath = Path.Combine(folder, fileName);
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await Grid1ImgFile.CopyToAsync(stream);
                        }
                        existingModel.Grid1ImageUrl = "/Banners/" + fileName;
                    }

                    existingModel.Grid2Title = model.Grid2Title;
                    existingModel.Grid2Subtitle = model.Grid2Subtitle;
                    existingModel.Grid2LinkUrl = model.Grid2LinkUrl;
                    existingModel.Grid2CategoryName = model.Grid2CategoryName;
                    if (Grid2ImgFile != null && Grid2ImgFile.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(Grid2ImgFile.FileName);
                        var filePath = Path.Combine(folder, fileName);
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await Grid2ImgFile.CopyToAsync(stream);
                        }
                        existingModel.Grid2ImageUrl = "/Banners/" + fileName;
                    }

                    existingModel.Grid3Title = model.Grid3Title;
                    existingModel.Grid3Subtitle = model.Grid3Subtitle;
                    existingModel.Grid3LinkUrl = model.Grid3LinkUrl;
                    existingModel.Grid3CategoryName = model.Grid3CategoryName;
                    if (Grid3ImgFile != null && Grid3ImgFile.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(Grid3ImgFile.FileName);
                        var filePath = Path.Combine(folder, fileName);
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await Grid3ImgFile.CopyToAsync(stream);
                        }
                        existingModel.Grid3ImageUrl = "/Banners/" + fileName;
                    }

                    existingModel.Grid4Title = model.Grid4Title;
                    existingModel.Grid4Subtitle = model.Grid4Subtitle;
                    existingModel.Grid4LinkUrl = model.Grid4LinkUrl;
                    existingModel.Grid4CategoryName = model.Grid4CategoryName;
                    if (Grid4ImgFile != null && Grid4ImgFile.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(Grid4ImgFile.FileName);
                        var filePath = Path.Combine(folder, fileName);
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await Grid4ImgFile.CopyToAsync(stream);
                        }
                        existingModel.Grid4ImageUrl = "/Banners/" + fileName;
                    }

                    existingModel.Grid5Title = model.Grid5Title;
                    existingModel.Grid5Subtitle = model.Grid5Subtitle;
                    existingModel.Grid5LinkUrl = model.Grid5LinkUrl;
                    existingModel.Grid5CategoryName = model.Grid5CategoryName;
                    if (Grid5ImgFile != null && Grid5ImgFile.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(Grid5ImgFile.FileName);
                        var filePath = Path.Combine(folder, fileName);
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await Grid5ImgFile.CopyToAsync(stream);
                        }
                        existingModel.Grid5ImageUrl = "/Banners/" + fileName;
                    }

                    _context.Banners.Update(existingModel);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = _localizer["BannersSavedSuccessfully"].Value;
                    return View(existingModel);
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, _localizer["ErrorSavingChanges"].Value + ": " + ex.Message);
                }
            }

            return View(model);
        }
    }
}
