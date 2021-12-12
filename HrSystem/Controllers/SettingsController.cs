using HrSystem.Constants;
using HrSystem.Data;
using HrSystem.Models;
using HrSystem.Seeds;
using HrSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace HrSystem.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        public ApplicationDbContext dbContext;
        public SettingsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            DefaultSettings.SeedGeneralSettings(dbContext);
        }

        [HasPermission("GeneralSetting", "View")]
        public IActionResult General()
        {
            GeneralSettingsViewModel viewModel = new GeneralSettingsViewModel();
            viewModel.extraDiscountSettings = dbContext.ExtraDiscountSettings.FirstOrDefault();
            viewModel.weeklyHolidays = dbContext.WeeklyHolidays.ToList();
            return View(viewModel);
        }

        [HttpPost]
        [HasPermission("GeneralSetting", "Edit")]
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
