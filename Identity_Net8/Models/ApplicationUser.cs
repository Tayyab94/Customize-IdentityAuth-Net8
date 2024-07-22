using Microsoft.AspNetCore.Identity;

namespace Identity_Net8.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Hobby { get; set; }   
    }
}
