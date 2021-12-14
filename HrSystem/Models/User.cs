using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HrSystem.Models
{
    public class User :  IdentityUser
    {
        [Required]
        public string FullName { get; set;}
    }
}
