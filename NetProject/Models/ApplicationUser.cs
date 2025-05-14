using Microsoft.AspNetCore.Identity;

namespace NetProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}