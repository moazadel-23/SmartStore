using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartStore.Models;
using SmartStore.Repositories;

namespace SmartStore.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Route("Cart")]
    public class CartController : Controller
    {
        private readonly IRepository<Cart> _cartRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Promotion> _promotionRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(
            IRepository<Cart> cartRepository,
            IRepository<Product> productRepository,
            IRepository<Promotion> promotionRepository,
            UserManager<ApplicationUser> userManager)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _promotionRepository = promotionRepository;
            _userManager = userManager;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            IEnumerable<Cart> cartItems = new List<Cart>();

            if (user != null)
            {
                cartItems = await _cartRepository.GetAsync(
                    c => c.UserId == user.Id,
                    include: [c => c.Product!]
                );
            }

            var allProducts = await _productRepository.GetAsync(tracked: false);
            ViewBag.AllProducts = allProducts.Select(p => new
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Discount = p.Discount,
                MainImg = p.MainImg,
                description = p.description
            }).ToList();

            return View(cartItems);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddToCart(int productId, int qty = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = true, isGuest = true });
            }

            var product = await _productRepository.GetOneAsync(p => p.Id == productId, tracked: false);
            if (product == null)
            {
                return Json(new { success = false, message = "Product not found" });
            }

            var cartItem = await _cartRepository.GetOneAsync(c => c.UserId == user.Id && c.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Count += qty;
                _cartRepository.Update(cartItem);
            }
            else
            {
                await _cartRepository.AddAsync(new Cart
                {
                    UserId = user.Id,
                    ProductId = productId,
                    Count = qty
                });
            }

            await _cartRepository.Commit();

            var cartItems = await _cartRepository.GetAsync(c => c.UserId == user.Id);
            var totalCount = cartItems.Sum(c => c.Count);

            return Json(new { success = true, isGuest = false, count = totalCount });
        }

        [HttpPost]
        [Route("UpdateQuantity")]
        public async Task<IActionResult> UpdateQuantity(int productId, int qty)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "User not logged in" });
            }

            var cartItem = await _cartRepository.GetOneAsync(c => c.UserId == user.Id && c.ProductId == productId);
            if (cartItem == null)
            {
                return Json(new { success = false, message = "Item not found in cart" });
            }

            if (qty <= 0)
            {
                _cartRepository.Delete(cartItem);
            }
            else
            {
                cartItem.Count = qty;
                _cartRepository.Update(cartItem);
            }

            await _cartRepository.Commit();

            var cartItems = await _cartRepository.GetAsync(
                expression: c => c.UserId == user.Id,
                include: new System.Linq.Expressions.Expression<System.Func<Cart, object>>[] { c => c.Product! }
            );

            var totalCount = cartItems.Sum(c => c.Count);
            var subtotal = cartItems.Sum(c => c.Count * c.Product!.Price);

            return Json(new { success = true, count = totalCount, subtotal = subtotal });
        }

        [HttpPost]
        [Route("Remove")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "User not logged in" });
            }

            var cartItem = await _cartRepository.GetOneAsync(c => c.UserId == user.Id && c.ProductId == productId);
            if (cartItem != null)
            {
                _cartRepository.Delete(cartItem);
                await _cartRepository.Commit();
            }

            var cartItems = await _cartRepository.GetAsync(
                c => c.UserId == user.Id,
                include: [c => c.Product!] 
            );

            var totalCount = cartItems.Sum(c => c.Count);
            var subtotal = cartItems.Sum(c => c.Count * c.Product!.Price);

            return Json(new { success = true, count = totalCount, subtotal = subtotal });
        }

        [HttpPost]
        [Route("SyncCart")]
        public async Task<IActionResult> SyncCart([FromBody] List<Cart> items)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "User not logged in" });
            }

            if (items != null && items.Any())
            {
                foreach (var item in items)
                {
                    var existing = await _cartRepository.GetOneAsync(c => c.UserId == user.Id && c.ProductId == item.ProductId);
                    if (existing != null)
                    {
                        existing.Count += item.Count;
                        _cartRepository.Update(existing);
                    }
                    else
                    {
                        var product = await _productRepository.GetOneAsync(p => p.Id == item.ProductId, tracked: false);
                        if (product != null)
                        {
                            await _cartRepository.AddAsync(new Cart
                            {
                                UserId = user.Id,
                                ProductId = item.ProductId,
                                Count = item.Count
                            });
                        }
                    }
                }

                await _cartRepository.Commit();
            }

            var cartItems = await _cartRepository.GetAsync(c => c.UserId == user.Id);
            var totalCount = cartItems.Sum(c => c.Count);

            return Json(new { success = true, count = totalCount });
        }

        [HttpPost]
        [Route("ApplyCoupon")]
        public async Task<IActionResult> ApplyCoupon([FromBody] CouponRequest request, CancellationToken cancellationToken)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Code))
            {
                return Json(new { success = false, message = "يرجى إدخال كود كوبون صالح" });
            }

            var promotion = await _promotionRepository.GetOneAsync(
                expression: p => p.Code.ToLower() == request.Code.ToLower() && p.IsValid && p.ValidTo > DateTime.Now,
                cancellationToken: cancellationToken
            );

            if (promotion == null)
            {
                return Json(new { success = false, message = "كود الخصم غير صالح أو منتهي الصلاحية" });
            }

            decimal totalDiscount = 0;
            bool couponApplied = false;

            if (request.Items != null && request.Items.Any())
            {
                foreach (var item in request.Items)
                {
                    var product = await _productRepository.GetOneAsync(p => p.Id == item.ProductId, tracked: false, cancellationToken: cancellationToken);
                    if (product == null) continue;

                    bool applies = false;
                    if (promotion.ProductId.HasValue)
                    {
                        applies = (product.Id == promotion.ProductId.Value);
                    }
                    else if (promotion.CategoryId.HasValue)
                    {
                        applies = (product.CategoryId == promotion.CategoryId.Value);
                    }
                    else if (promotion.BrandId.HasValue)
                    {
                        applies = (product.BrandId == promotion.BrandId.Value);
                    }
                    else
                    {
                        // General promotion applies to everything
                        applies = true;
                    }

                    if (applies)
                    {
                        // Calculate item discount based on discount percentage
                        decimal itemDiscount = (product.Price * (promotion.Discount / 100)) * item.Count;
                        totalDiscount += itemDiscount;
                        couponApplied = true;
                    }
                }
            }

            if (!couponApplied)
            {
                return Json(new { success = false, message = "هذا الكوبون لا ينطبق على أي من المنتجات الموجودة في السلة" });
            }

            return Json(new { 
                success = true, 
                discount = totalDiscount, 
                message = $"تم تطبيق كوبون خصم {promotion.Discount.ToString("G29")}% بنجاح!" 
            });
        }
    }

    public class CouponRequest
    {
        public string Code { get; set; } = string.Empty;
        public List<CartItemDto> Items { get; set; } = new();
    }

    public class CartItemDto
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
    }
}
