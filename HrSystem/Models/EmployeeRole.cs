using Microsoft.AspNetCore.Identity;

namespace HrSystem.Models
{
    public class UserRole:IdentityRole
    {
        public UserRole()
        {
        }
        public UserRole(string roleName) : base(roleName)
        {
        }

        public string? Dummy { get; set; }
    }
}
