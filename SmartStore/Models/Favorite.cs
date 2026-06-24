using System.ComponentModel.DataAnnotations;

namespace SmartStore.Models
{
    public class Favorite
    {
        public string UserId { get; set; } = null!;
        public int ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;
    }
}
