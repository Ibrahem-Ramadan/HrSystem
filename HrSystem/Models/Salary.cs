using System.ComponentModel.DataAnnotations.Schema;

namespace HrSystem.Models
{
    public class Salary
    {
        public Double TotalSalary { get; set; }
        public Double MonthlyBouns { get; set; }
        public Double MonthlyDiscount { get; set; }
        public int Month  { get; set; }
        public int Year { get; set; }

        [ForeignKey("employee")]
        public string employeeId { get; set; }
        public virtual Employee employee { get; set; }


    }
}
