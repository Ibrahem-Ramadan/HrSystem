using System.ComponentModel.DataAnnotations;

namespace HrSystem.ViewModels
{
    public class EmployeeVM
    {

        public string id { get; set; }
        public string FullName { get; set; }
       
      
   
        public string PhoneNumber { get; set; }
        [Required]
        public double SalaryAmount { get; set; }
        [Required]
        public string JopTitle { get; set; }
      
       
        public char Gender { get; set; }
        [Required]

     
        public TimeSpan CheckOutTime { get; set; }
        [Required]
        public TimeSpan AttendanceTime { get; set; }

        public string GroupsNames { get; set; }


    }
}
