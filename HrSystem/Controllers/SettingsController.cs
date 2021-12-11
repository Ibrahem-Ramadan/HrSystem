using HrSystem.Data;
using HrSystem.Models;
using HrSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace HrSystem.Controllers
{
    public class SettingsController : Controller
    {
        public ApplicationDbContext dbContext;
        public SettingsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult General()
        {
            GeneralSettingsViewModel viewModel = new GeneralSettingsViewModel();
            viewModel.extraDiscountSettings = dbContext.ExtraDiscountSettings.FirstOrDefault();
            viewModel.weeklyHolidays = dbContext.WeeklyHolidays.ToList();

            var weekinit = new List<WeeklyHoliday>()
            {
                new WeeklyHoliday(1, "Sturday", false),
                new WeeklyHoliday(2, "Sunday", false),
                new WeeklyHoliday(3, "Monday", false),
                new WeeklyHoliday(4, "Tuesday", false),
                new WeeklyHoliday(5, "Wednesday", false),
                new WeeklyHoliday(6, "Thursday", false),
                new WeeklyHoliday(7, "Friday", false),

            };

            ExtraDiscountSetting extradiscountiinit = new ExtraDiscountSetting(1, 0, 0, "Hours");

            if (viewModel.extraDiscountSettings == null)
            {
                viewModel.extraDiscountSettings = extradiscountiinit;
            }

            if (viewModel.weeklyHolidays == null)
            {
                viewModel.weeklyHolidays = weekinit;
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult General(GeneralSettingsViewModel viewModel)
        {
            if (viewModel is null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            dbContext.Entry(viewModel.extraDiscountSettings).State = EntityState.Modified;
            foreach (var weeklyHoliday in viewModel.weeklyHolidays)
            {
                dbContext.Entry(weeklyHoliday).State = EntityState.Modified;
            }

            dbContext.SaveChanges();
            return RedirectToAction("general");
        }
    }
}
