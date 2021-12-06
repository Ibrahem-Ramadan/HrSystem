using System.ComponentModel.DataAnnotations.Schema;

namespace HrSystem.Models
{
    public class RolesPermession
    {
        
        public Permissions permissions { get; set; }
        [ForeignKey("employeeRole")]
        public string RoleId { get; set; }
        [ForeignKey("permissions")]
        public int PermissionId { get; set; }
        
        public EmployeeRole employeeRole { get; set; }
        
    }
}
