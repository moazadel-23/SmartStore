namespace SmartStore.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string MainImg { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal Rate { get; set; }
        public decimal Quantity { get; set; }
        public decimal Discount { get; set; }
        public bool Status { get; set; }
        public Brand? Brand { get; set; }
        public int BrandId { get; set; }
        public Category? Category { get; set; }
        public int CategoryId { get; set; }
        public ICollection<ProductImges> ProductSubImgs { get; set; } = new List<ProductImges>();
    }
}
