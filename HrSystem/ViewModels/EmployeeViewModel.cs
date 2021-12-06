using System.ComponentModel.DataAnnotations;

namespace HrSystem.ViewModels
{
    public class EmployeeViewModel
    {
        [Required]
        [MaxLength(50,ErrorMessage ="InValid Length !!")]
        [MinLength(3)]
        
        public string FirstName { get; set; }
        public string? Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "InValid Length !!")]
        [MinLength(3)]
        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^01[0-2]\d{1,8}$", ErrorMessage = "Invalid Phone")]
        public string PhoneNumber { get; set; }
        [Required]
        public double SalaryAmount { get; set; }
        [Required]
        public string JopTitle { get; set; }
        public string? ProfilePicture { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string SSN { get; set; }
        [Required]

        public string Nationality { get; set; }
        public string? Notes { get; set; }
        [Required]
        public char Gender { get; set; }
        [Required]
        public DateTime EmploymentDate { get; set; }
        [Required]
        public DateTime BirthOfDate { get; set; }
        [Required]
        public TimeSpan CheckOutTime { get; set; }
        [Required]
        public TimeSpan AttendanceTime { get; set; }
        [Required]
        public int deptId { get; set; }
      

      

    }
}
