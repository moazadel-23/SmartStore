using System.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SmartStore.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        [ValidateNever]
        public string MainImg { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal Rate { get; set; }
        public decimal Quantity { get; set; }
        public decimal Discount { get; set; }
        public bool Status { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        [ValidateNever]
        public Brand? Brand { get; set; }
        public int BrandId { get; set; }
        [ValidateNever]
        public Category? Category { get; set; }
        public int CategoryId { get; set; }
        [ValidateNever]
        public ICollection<ProductImges> ProductSubImgs { get; set; } = new List<ProductImges>();
        
        public string? SKU { get; set; }
        public string? Warranty { get; set; }
        public string? ReturnPolicy { get; set; }
        public string? DeliveryTimeCairoGiza { get; set; }
        public string? DeliveryTimeOutside { get; set; }
        public string? PackageContents { get; set; }
        public string? OverviewTitle { get; set; }
        public string? OverviewDescription { get; set; }
        public string? OverviewImageUrl { get; set; }
        
        [ValidateNever]
        public ICollection<ProductSpecification> Specifications { get; set; } = new List<ProductSpecification>();
        
        [ValidateNever]
        public ICollection<ProductReview> Reviews { get; set; } = new List<ProductReview>();
    }
}
