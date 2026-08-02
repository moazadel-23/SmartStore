using System.Collections.Generic;
using SmartStore.Models;

namespace SmartStore.ViewModel
{
    public class DashboardViewModel
    {
        public int TotalOrdersCount { get; set; }
        public int TotalUsersCount { get; set; }
        public int TotalProductsCount { get; set; }
        public List<Order>? RecentOrders { get; set; } 
        public Order? Order { get; set; }
        public int TotalProduct { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
