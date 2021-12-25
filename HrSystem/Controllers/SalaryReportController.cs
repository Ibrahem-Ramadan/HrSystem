using HrSystem.Data;
using HrSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrSystem.Controllers
{
    [Authorize]
    public class SalaryReportController : Controller
    {
        private ApplicationDbContext _dbContext;

        public SalaryReportController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        // GET: SalaryReportController
        [HasPermission("SalaryReport","View")]
        public ActionResult Index()
        { 
            return View();
        }

        public ActionResult SalaryReport(int? _month , int? _year)
        {
            int? year = _year==null ? DateTime.Now.Year:_year;
            int? month = _month==null ?DateTime.Now.Month:_month;
            List<SalaryReportViewModel> salaryReports = new List<SalaryReportViewModel>();

            var ExtraDiscountSetting = _dbContext.ExtraDiscountSettings.FirstOrDefault();
            var employees = _dbContext.Employees.ToList();
            var attendances = _dbContext.Attendances.Where(a => a.AttendanceDate.Month == month && a.AttendanceDate.Year == year).ToList();

            foreach (var employee in employees)
            {
                var EmpAttendances = attendances.Where(a => a.EmployeeId == employee.Id).ToList();
                if (EmpAttendances.Count != 0)
                {
                    int NumOfEmpAttendanceDays = EmpAttendances.Where(a => a.IsAttend).Count();
                    int NumOfEmpAbsentDays = EmpAttendances.Where(a => !a.IsAttend).Count();
                    int WorkHours = (int)employee.CheckOutTime.Value.Subtract(employee.AttendanceTime.Value).TotalHours;

                    double salaryPerday = employee.SalaryAmount / (30 - _dbContext.WeeklyHolidays.Where(w => w.IsHoliday).Count() * 4);
                    double salaryPerhour = salaryPerday / WorkHours;


                    //OverTimeHours Calculation
                    int OverTimeHoursOverInterval = 0;
                    int DiscountTimeHoursOverInterval = 0;

                    foreach (var attendance in EmpAttendances.Where(a => a.IsAttend).ToList())
                    {
                        OverTimeHoursOverInterval += attendance.Overtime;
                        DiscountTimeHoursOverInterval += attendance.Discount;
                    }

                    double totalBounus = 0.0, totalDiscount = 0.0;
                    if (ExtraDiscountSetting.SettingType == "Hours")
                    {
                        totalBounus = Convert.ToInt32(OverTimeHoursOverInterval / 60) * ExtraDiscountSetting.Extra * salaryPerhour;
                        totalDiscount = (Convert.ToInt32(DiscountTimeHoursOverInterval / 60) * ExtraDiscountSetting.Discount * salaryPerhour) + (NumOfEmpAbsentDays * salaryPerday);
                    }
                    else // Money
                    {
                        totalBounus = Convert.ToInt32(OverTimeHoursOverInterval / 60) * ExtraDiscountSetting.Extra;
                        totalDiscount = (Convert.ToInt32(DiscountTimeHoursOverInterval / 60) * ExtraDiscountSetting.Discount) + (NumOfEmpAbsentDays * salaryPerday);
                    }

                    salaryReports.Add(new SalaryReportViewModel
                    {
                        EmployeeId = employee.Id,
                        EmployeeName = $"{employee.FirstName} {employee.LastName}",
                        AttendanceDays = NumOfEmpAttendanceDays,
                        AbsentDays = NumOfEmpAbsentDays,
                        Salary = employee.SalaryAmount,
                        OvertimeHours = OverTimeHoursOverInterval / 60,
                        DiscountHours = DiscountTimeHoursOverInterval / 60,
                        Bonus = totalBounus,
                        Discount = totalDiscount,
                        Total = ((NumOfEmpAttendanceDays + NumOfEmpAbsentDays) * salaryPerday) + (totalBounus - totalDiscount)
                    });
                }

            }
            return PartialView(salaryReports);
        }

        // GET: SalaryReportController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


    }
}
