using System.ComponentModel.DataAnnotations;

namespace SmartStore.Models
{
    public enum OrderStatus
    {
        [Display(Name = "Pending")]
        Pending,
        [Display(Name = "Processing")]
        Processing,
        [Display(Name = "Shipped")]
        Shipped,
        [Display(Name = "Delivered")]
        Delivered,
        [Display(Name = "Returned")]
        Returned,
        [Display(Name = "Cancelled")]
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
