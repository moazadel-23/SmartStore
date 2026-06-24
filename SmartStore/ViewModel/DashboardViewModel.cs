namespace SmartStore.ViewModel
{
    public class DashboardViewModel
    {
        internal int totalOrder;

        public int TotalUsersCount { get; set; }
        public int TotalProductsCount { get; set; }
        public List<Order>? RecentOrders { get; set; } 
        public Order? Order { get; set; }
        public int TotalProduct { get; set; }
    }
}
