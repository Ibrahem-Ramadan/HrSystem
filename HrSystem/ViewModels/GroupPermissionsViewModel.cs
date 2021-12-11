#nullable disable
using System.ComponentModel.DataAnnotations;

namespace HrSystem.ViewModels
{
    public class GroupPermissionsViewModel
    {
        public GroupPermissionsViewModel()
        {
            PermissionsModules = new List<PermissionsModuleViewModel>();
        }
        [Required]
        public string GroupName { get; set; }
        public List<PermissionsModuleViewModel> PermissionsModules { get; set; }
    }
}
