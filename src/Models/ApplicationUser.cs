using Microsoft.AspNetCore.Identity;

namespace SimpleApi.src.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsActive { get; set; } = true;
    }
}
