using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartStore.Models;
using SmartStore.Repositories;
using SmartStore.ViewModel;

namespace SmartStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashBoardController : Controller
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Product> _productRepository;

        public DashBoardController(
            IRepository<Order> orderRepository,
            UserManager<ApplicationUser> userManager,
            IRepository<Product> productRepository)
        {
            _orderRepository = orderRepository;
            _userManager = userManager;
            _productRepository = productRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var totalUsers = await _userManager.Users.CountAsync();

            var totalOrder = await _orderRepository.CountAsync();

           // var recentOrders = allOrders.OrderByDescending(o => o.OrderDate).Take(5).ToList();

            var totalProducts = await _productRepository.CountAsync();

            var viewModel = new DashboardViewModel
            {
                totalOrder = totalOrder,
                TotalProduct = totalProducts,
                TotalUsersCount = totalUsers,
               // RecentOrders = recentOrders
            };
            return View(viewModel);
        }
    }
}
