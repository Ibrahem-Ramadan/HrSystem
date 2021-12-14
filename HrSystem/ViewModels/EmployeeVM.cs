using System.ComponentModel.DataAnnotations;

namespace HrSystem.ViewModels
{
    public class EmployeeVM
    {

        public string id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public double SalaryAmount { get; set; }
        public string? JopTitle { get; set; }
      
        public char? Gender { get; set; }
        public TimeSpan? CheckOutTime { get; set; }
        public TimeSpan ?AttendanceTime { get; set; }

    }
}
