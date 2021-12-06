using System.ComponentModel.DataAnnotations;

namespace HrSystem.Models
{
    public class Department
    {
        [Key]
        public int DeptId { get; set; }
        public string DeptName { get; set; }
        public int WeaklyWorkDays { get; set; }
        public  virtual ICollection<Employee> employees { get; set; }



    }
}
