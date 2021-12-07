using HrSystem.Models;

namespace HrSystem.ViewModels
{
    public class GeneralSettingsViewModel
    {
        public List<ExtraDiscountSetting>? extraDiscountSettings { get; set; }
        public List<WeeklyHoliday>? weeklyHolidays { get; set; }
    }
}
