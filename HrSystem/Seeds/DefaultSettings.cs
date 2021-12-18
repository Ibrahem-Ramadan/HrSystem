using HrSystem.Data;
using HrSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HrSystem.Seeds
{
    public static class DefaultSettings
    {
        public static void SeedGeneralSettings(ApplicationDbContext dbContext)
        {

            if (!dbContext.WeeklyHolidays.Any())
            {
                var weekinit = new List<WeeklyHoliday>()
                {
                    new WeeklyHoliday("Sunday", false),
                    new WeeklyHoliday("Monday", false),
                    new WeeklyHoliday("Tuesday", false),
                    new WeeklyHoliday("Wednesday", false),
                    new WeeklyHoliday("Thursday", false),
                    new WeeklyHoliday("Friday", false),
                    new WeeklyHoliday("Sturday", false),

                };

                foreach (var item in weekinit)
                {
                    dbContext.Add(item);
                }

            }

            if (!dbContext.ExtraDiscountSettings.Any())
            {
                dbContext.Add(new ExtraDiscountSetting(0, 0, "Hours"));
            }
            dbContext.SaveChanges();
        }

         
    }
}
