using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrSystem.Models
{
	public class OfficialHoliday
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int HolidayId { get; set; }

		[Required]
		[MaxLength(150, ErrorMessage = "InValid Length !!")]
		[MinLength(5)]
		public string HolidayName { get; set; }
		[Required]
		public DateTime HolidayDate { get; set; }
	}
}
