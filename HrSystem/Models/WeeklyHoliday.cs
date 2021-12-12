using System.ComponentModel.DataAnnotations;

namespace HrSystem.Models
{
    public class WeeklyHoliday
    {
        [Key]
        public int Id { get; set; }
        public string Day { get; set; }
        public bool IsHoliday { get; set; }
        public WeeklyHoliday()
        {

        }
        public WeeklyHoliday(string Day, bool IsHoliday)
        {
            this.Day = Day;
            this.IsHoliday = IsHoliday;
        }
    }
}
