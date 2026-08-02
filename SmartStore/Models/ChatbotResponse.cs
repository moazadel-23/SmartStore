using SmartStore.Areas.Customer.Controllers;

namespace SmartStore.Models
{
    public class ChatbotResponse
    {
        public string Message { get; set; } = string.Empty;
        public string NextState { get; set; } = string.Empty;
        public List<ProductDto>? Products { get; set; }
        public List<string>? Options { get; set; }
    }
}
