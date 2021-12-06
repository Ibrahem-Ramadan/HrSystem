using System.ComponentModel.DataAnnotations;

namespace HrSystem.Models
{
    public class ExtraDiscountSetting
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public float Extra { get; set; }  
        [Required]
        public float Discount { get; set; }
        public string? SettingType { get; set; }    // Hours or Money
    }
}
