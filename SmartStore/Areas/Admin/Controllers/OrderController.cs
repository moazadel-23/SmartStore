using Microsoft.AspNetCore.Mvc;
using SmartStore.Repositories;
using System.Linq.Expressions;

namespace SmartStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IRepository<Order> _orderRepository;

        public OrderController(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepository.GetAsync();
            return View(orders.OrderByDescending(o => o.OrderDate));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderRepository.GetOneAsync(
                e => e.Id == id,
                new Expression<Func<Order, object>>[] { o => o.OrderItems }
            );

            if (order == null) return NotFound();

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, OrderStatus orderStatus, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOneAsync(e => e.Id == id);
            if (order == null) return NotFound();

            order.OrderStatus = orderStatus;
            _orderRepository.Update(order);
            await _orderRepository.Commit(cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOneAsync(e => e.Id == id);
            if (order == null) return NotFound();

            _orderRepository.Delete(order);
            await _orderRepository.Commit(cancellationToken);

            return RedirectToAction(nameof(Index));
        }
    }
}
