using Microsoft.AspNetCore.Mvc;
using SmartStore.Models;
using SmartStore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace SmartStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashBoardController : Controller
    {
        private readonly IRepository<Order> _orderRepository;

        public DashBoardController(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IActionResult> Index()
        {
            var allOrders = await _orderRepository.GetAsync();
            var recentOrders = allOrders.OrderByDescending(o => o.OrderDate).Take(5).ToList();
            return View(recentOrders);
        }
    }
}
