using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrSystem.Models
{
    public class Vacation
    {
        [Key]
        public int VacationId { get; set; }
        public string VacationTitle { get; set; }
        public string VacationType { get; set; }
       
        [Remote("check", "Vacations", ErrorMessage = "ssn is not exist !!")]
        public string SSN { get; set; }
        [Required]
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Status { get; set; }

        [ForeignKey("Employee")]
        public string EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
