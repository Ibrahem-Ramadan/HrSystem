using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrSystem.Models
{
    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }
        public Boolean IsAttend { get; set; }
        public DateTime AttendanceDate { get; set; }
        public TimeSpan AttendanceTime { get; set; }
        public TimeSpan LeaveTime { get; set; }
        public int Overtime { get; set; }
        public int Discount { get; set; }
        [ForeignKey("Employee")]
        [Required]
        public string EmployeeId { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
