using Microsoft.AspNetCore.Mvc;
using SmartStore.Repositories;

namespace SmartStore.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Route("Customer/[controller]/[action]")]
    public class ChatbotController : Controller
    {
        private readonly IRepository<Product> _productRepository;

        public ChatbotController(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPost]
        public async Task<IActionResult> GetResponse([FromBody] ChatMessageRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var text = request.Message?.Trim().ToLower() ?? "";
            var state = request.State ?? "";

            // Check if user is currently searching for a product
            if (state == "waiting_for_search" && !string.IsNullOrWhiteSpace(text) && text != "❌ إلغاء البحث" && text != "cancel")
            {
                // Find products matching the search query
                var allProducts = await _productRepository.GetAsync(
                    expression: p => p.Name.Contains(text) || p.description.Contains(text),
                    tracked: false
                );

                var productsList = allProducts.Take(5).Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    MainImg = p.MainImg
                }).ToList();

                if (productsList.Any())
                {
                    return Json(new ChatbotResponse
                    {
                        Message = $"لقد وجدت {productsList.Count} منتجات تطابق بحثك:",
                        NextState = "",
                        Products = productsList,
                        Options = GetDefaultOptions()
                    });
                }
                else
                {
                    return Json(new ChatbotResponse
                    {
                        Message = $"عذراً، لم أجد أي منتجات تطابق '{request.Message}'. هل تريد تجربة البحث عن منتج آخر؟",
                        NextState = "waiting_for_search",
                        Options = new List<string> { "❌ إلغاء البحث" }
                    });
                }
            }

            // Quick Reply Options / Initial Intents
            if (text == "🔍 البحث عن منتج" || text == "search")
            {
                return Json(new ChatbotResponse
                {
                    Message = "من فضلك اكتب اسم المنتج الذي تبحث عنه وسأقوم بالبحث عنه فوراً!",
                    NextState = "waiting_for_search",
                    Options = new List<string> { "❌ إلغاء البحث" }
                });
            }
            else if (text == "🔄 سياسة الاسترجاع" || text == "return")
            {
                return Json(new ChatbotResponse
                {
                    Message = "يمكنك استرجاع أو استبدال أي منتج خلال 14 يوماً من الاستلام بشرط أن يكون في حالته الأصلية وبغلافه الأصلي مع الفاتورة. الاستبدال مجاني في حال وجود عيب صناعة.",
                    NextState = "",
                    Options = GetDefaultOptions()
                });
            }
            else if (text == "💳 طرق الدفع" || text == "payment")
            {
                return Json(new ChatbotResponse
                {
                    Message = "نوفر لك عدة طرق آمنة للدفع:\n1. 💵 الدفع عند الاستلام.\n2. 💳 الدفع بالبطاقات الائتمانية (Visa / MasterCard).\n3. 📱 الدفع عبر فودافون كاش.",
                    NextState = "",
                    Options = GetDefaultOptions()
                });
            }
            else if (text == "📦 تتبع الطلبات" || text == "order")
            {
                return Json(new ChatbotResponse
                {
                    Message = "لتتبع طلبك، يرجى تسجيل الدخول والذهاب إلى صفحة حسابك الشخصي لرؤية حالة الطلبات الحالية، أو يمكنك التواصل مع خدمة العملاء وتزويدهم برقم الطلب.",
                    NextState = "",
                    Options = GetDefaultOptions()
                });
            }
            else if (text == "📞 خدمة العملاء" || text == "support")
            {
                return Json(new ChatbotResponse
                {
                    Message = "فريق خدمة العملاء جاهز لمساعدتك! يمكنك الاتصال بنا على الرقم 19999 أو مراسلتنا مباشرة عبر الواتساب طوال أيام الأسبوع.<br><br><a href='https://wa.me/201019519390' target='_blank' class='chatbot-whatsapp-link' style='display:inline-flex;align-items:center;gap:8px;background-color:#25D366;color:white;padding:10px 18px;border-radius:25px;font-weight:bold;text-decoration:none;box-shadow:0 4px 12px rgba(37,211,102,0.3);'><i class=\"fab fa-whatsapp\" style=\"font-size:16px;\"></i> تواصل عبر واتساب</a>",
                    NextState = "",
                    Options = GetDefaultOptions()
                });
            }
            else if (text == "🟢 تواصل عبر واتساب" || text == "whatsapp")
            {
                return Json(new ChatbotResponse
                {
                    Message = "يسعدنا تواصلك معنا مباشرة عبر واتساب! اضغط على الزر أدناه لبدء المحادثة:<br><br><a href='https://wa.me/201019519390' target='_blank' class='chatbot-whatsapp-link' style='display:inline-flex;align-items:center;gap:8px;background-color:#25D366;color:white;padding:10px 18px;border-radius:25px;font-weight:bold;text-decoration:none;box-shadow:0 4px 12px rgba(37,211,102,0.3);'><i class=\"fab fa-whatsapp\" style=\"font-size:16px;\"></i> تواصل عبر واتساب</a>",
                    NextState = "",
                    Options = GetDefaultOptions()
                });
            }
            else if (text == "❌ إلغاء البحث" || text == "cancel")
            {
                return Json(new ChatbotResponse
                {
                    Message = "تم إلغاء البحث. كيف يمكنني مساعدتك الآن؟",
                    NextState = "",
                    Options = GetDefaultOptions()
                });
            }

            // Default Greeting
            return Json(new ChatbotResponse
            {
                Message = "مرحباً بك في متجر SmartStore! أنا مساعدك الذكي. كيف يمكنني مساعدتك اليوم؟ يرجى اختيار أحد الخيارات التالية:",
                NextState = "",
                Options = GetDefaultOptions()
            });
        }

        private List<string> GetDefaultOptions()
        {
            return new List<string>
            {
                "🔍 البحث عن منتج",
                "📦 تتبع الطلبات",
                "💳 طرق الدفع",
                "🔄 سياسة الاسترجاع",
                "🟢 تواصل عبر واتساب",
                "📞 خدمة العملاء"
            };
        }
    }
}
