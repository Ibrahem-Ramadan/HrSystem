using Microsoft.AspNetCore.Identity;

namespace HrSystem.Models
{
    public class EmployeeRole:IdentityRole
    {
        public virtual ICollection<RolesPermession> RolesPermessions { get; set; }
    }
}
