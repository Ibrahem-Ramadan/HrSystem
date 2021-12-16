using HrSystem.Models;

namespace HrSystem.ViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Roles = new List<CheckBoxViewModel>();
            User = new User();
        }
        public User User { get; set; }
        public List<CheckBoxViewModel> Roles { get; set; }
    }
}
