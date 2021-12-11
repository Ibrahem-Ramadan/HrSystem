using HrSystem.Models;

namespace HrSystem.ViewModels
{
    public class GeneralSettingsViewModel
    {
        public ExtraDiscountSetting extraDiscountSettings { get; set; }
        public List<WeeklyHoliday> weeklyHolidays { get; set; }
    }
}
