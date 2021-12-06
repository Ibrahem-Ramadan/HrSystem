using System.ComponentModel.DataAnnotations;

namespace HrSystem.Models
{
    public class WeeklyHoliday
    {
        [Key]
        public int Id { get; set; }
        public string Day { get; set; }
        public bool IsHoliday { get; set; }
    }
}
