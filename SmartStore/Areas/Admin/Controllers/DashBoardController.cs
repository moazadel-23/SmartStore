using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartStore.Models;
using SmartStore.Repositories;
using SmartStore.ViewModel;
using System.Linq;
using System.Threading.Tasks;

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
            var totalProducts = await _productRepository.CountAsync();

            var allOrders = await _orderRepository.GetAsync();
            var totalOrder = allOrders.Count();
            var totalRevenue = allOrders.Sum(o => o.TotalAmount);
            var recentOrders = allOrders.OrderByDescending(o => o.OrderDate).Take(5).ToList();

            var viewModel = new DashboardViewModel
            {
                TotalOrdersCount = totalOrder,
                TotalProduct = totalProducts,
                TotalUsersCount = totalUsers,
                RecentOrders = recentOrders,
                TotalRevenue = totalRevenue
            };
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Profile()
        {
            return View();
        }
    }
}
