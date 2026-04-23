namespace SmartStore.Models
{
    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Returned,
        Cancelled
    }   
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending; // Pending, Processing, Shipped, Delivered, Cancelled
        
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
