using Microsoft.AspNetCore.Identity;

namespace HrSystem.Models
{
    public class EmployeeRole:IdentityRole
    {
        public EmployeeRole()
        {
        }
        public EmployeeRole(string roleName) : base(roleName)
        {
        }

        public string? Dummy { get; set; }
    }
}
