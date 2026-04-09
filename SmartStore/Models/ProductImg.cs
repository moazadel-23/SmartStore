using Microsoft.EntityFrameworkCore;

namespace SmartStore.Models
{
    [PrimaryKey(nameof(ProductId), nameof(subImg))]
    public class ProductImg
    {
        public int ProductId { get; set; }
        public  string subImg { get; set; } = string.Empty;
    }
}
