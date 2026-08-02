using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;

namespace SmartStore.Models
{
    public class ProductReview
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        
        [ValidateNever]
        public Product? Product { get; set; }
        
        public string CustomerName { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
