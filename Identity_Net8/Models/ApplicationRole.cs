
using Microsoft.AspNetCore.Identity;

namespace Identity_Net8.Models
{
    public class ApplicationRole :IdentityRole
    {
        public bool IsActive { get; set; }
    }
}
