using Microsoft.AspNetCore.Identity;

namespace URLShorten.Models
{
    public class UserRoleManager
    {
        public List<IdentityUser> Users { get; set; }
        public List<IdentityRole> Roles { get; set; }
    }
}

