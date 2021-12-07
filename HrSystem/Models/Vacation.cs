using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrSystem.Models
{
    public class Vacation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VacationId { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "InValid Length !!")]
        [MinLength(5)]
        public string VacationTitle { get; set; }
        [Required]
        public string VacationType { get; set; }
        [NotMapped]
        [Remote("check", "Vacations", ErrorMessage = "ssn is not exist !!")]
        public string SSN { get; set; }
        [Required]
        public DateTime DateFrom { get; set; }
        [Required]
        public DateTime DateTo { get; set; }
        [Required]
        public string Status { get; set; }

        [ForeignKey("Employee")]
        public string? EmployeeId { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
