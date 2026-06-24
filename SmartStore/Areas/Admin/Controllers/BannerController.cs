using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public BannerController(ApplicationDbContext context)
        {
            _context = context;
        }

        private BannerModel GetDefaultBanners()
        {
            return new BannerModel
            {
                Slide1ImageUrl = "https://images.unsplash.com/photo-1468495244123-6c6c332eeece?w=1600&fit=crop&q=80",
                Slide1LinkUrl = "/Customer/Home/Store",
                Slide2ImageUrl = "https://images.unsplash.com/photo-1556911220-e15b29be8c8f?w=1600&fit=crop&q=80",
                Slide2LinkUrl = "/Customer/Home/Store",
                Slide3ImageUrl = "https://images.unsplash.com/photo-1542751371-adc38448a05e?w=1600&fit=crop&q=80",
                Slide3LinkUrl = "/Customer/Home/Store",

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
                Installment2LinkUrl = "/Customer/Home/Store"
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
            IFormFile? HuaweiImgFile, IFormFile? AnkerImgFile)
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

                    _context.Banners.Update(existingModel);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "تم حفظ تعديلات البانرات بنجاح!";
                    return View(existingModel);
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "حدث خطأ أثناء حفظ التعديلات: " + ex.Message);
                }
            }

            return View(model);
        }
    }
}
