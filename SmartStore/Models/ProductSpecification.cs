using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SmartStore.Models
{
    public class ProductSpecification
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        
        [ValidateNever]
        public Product? Product { get; set; }
        
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        
        public bool IsHighlight { get; set; }
    }
}
