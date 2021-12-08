using Microsoft.AspNetCore.Identity;

namespace HrSystem.Models
{
    public class EmployeeRole:IdentityRole
    {
        public EmployeeRole()
        {
            RolesPermessions = new List<RolesPermession>();
        }
        public EmployeeRole(string roleName) : base(roleName)
        {
            RolesPermessions = new List<RolesPermession>();
        }

        public virtual ICollection<RolesPermession> RolesPermessions { get; set; }
    }
}
