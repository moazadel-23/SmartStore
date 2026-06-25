using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SmartStore.Models
{
    public class Promotion
    {
        public int Id { get; set; }

        [ValidateNever]
        public int? ProductId { get; set; }
        [ValidateNever]
        public Product? Product { get; set; }

        [ValidateNever]
        public int? CategoryId { get; set; }
        [ValidateNever]
        public Category? Category { get; set; }

        [ValidateNever]
        public int? BrandId { get; set; }
        [ValidateNever]
        public Brand? Brand { get; set; }

        public string Code { get; set; } = string.Empty;
        public decimal Discount { get; set; }
        public bool IsValid { get; set; }
        public DateTime ValidTo { get; set; }
        public DateTime PublishAt { get; set; } = DateTime.UtcNow;
    }
}
