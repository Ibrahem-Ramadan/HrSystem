namespace HrSystem.ViewModels
{
    public class SalaryReportViewModel
    {

        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public double Salary { get; set; }
        public int AttendanceDays { get; set; }
        public int AbsentDays { get; set; }
        public int OvertimeHours { get; set; }
        public int DiscountHours { get; set; }
        public double Bonus { get; set; }
        public double Discount { get; set; }
        public double Total { get; set; }

    }
}
