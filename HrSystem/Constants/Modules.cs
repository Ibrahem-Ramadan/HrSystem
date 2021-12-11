using System.ComponentModel.DataAnnotations;

namespace HrSystem.Constants
{
    public enum Modules
    {
        Employees,
        [Display(Name = "General Setting")]
        GeneralSetting,
        Groups,
        Attendance,
        [Display(Name = "Salary Report")]
        SalaryReport,
        Vacations,
        [Display(Name = "Official Holidays")]
        OfficialHolidays,
    }
}
