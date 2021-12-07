using HrSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using HrSystem.ViewModels;
using HrSystem.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HrSystem.Controllers
{
    public class HomeController : Controller
    {
        public ApplicationDbContext dbContext;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult GeneralSettings()
        {
            GeneralSettingsViewModel viewModel = new GeneralSettingsViewModel();
            viewModel.extraDiscountSettings = dbContext.ExtraDiscountSettings.ToList();
            viewModel.weeklyHolidays = dbContext.WeeklyHolidays.ToList();

            return View(viewModel);
        }
    }
}