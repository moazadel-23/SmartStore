using Microsoft.EntityFrameworkCore;

namespace SmartStore.Models
{
    public class ProductImges
    {
        public int Id { get; set; } 
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public string SubImg { get; set; } = string.Empty;
    }
}
