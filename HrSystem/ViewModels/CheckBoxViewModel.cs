#nullable disable
using System.ComponentModel.DataAnnotations;

namespace HrSystem.ViewModels
{
    public class CheckBoxViewModel
    {
        public string DisplayValue { get; set; }
        public bool IsSelected {get; set;} =false;
    }
}
 