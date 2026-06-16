using System.ComponentModel.DataAnnotations;

namespace SmartStore.ViewModel
{
    public class loginVM
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }
}
