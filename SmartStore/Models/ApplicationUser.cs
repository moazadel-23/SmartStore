using Microsoft.AspNetCore.Identity;

namespace SmartStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string ProfileImg { get; set; } = string.Empty;

    }
}
