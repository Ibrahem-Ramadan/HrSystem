

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrSystem.Models
{
    public class Employee : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        public double SalaryAmount { get; set; }
        public string JopTitle { get; set; }
        public string? ProfilePicture { get; set; }
        public string Address { get; set; }
        public string SSN { get; set; }
      
        public string Nationality { get; set; }
        public string Notes { get; set; }
        public char Gender { get; set; }
        public DateTime EmploymentDate { get; set; }
        public DateTime BirthOfDate { get; set; }
        public TimeSpan CheckOutTime { get; set; }
        public TimeSpan AttendanceTime { get; set; }

        [ForeignKey("department")]
        public int deptId { get; set; }
        public virtual Department department { get; set; }

        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<Vacation> Vacations { get; set; }
        public virtual Salary salary { get; set; }


    }
}
