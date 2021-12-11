using Microsoft.AspNetCore.Mvc;

namespace HrSystem.Controllers
{
    public class HasPermissionAttribute : TypeFilterAttribute
    {
        public HasPermissionAttribute(string module, string permmision)
          : base(typeof(HasPermissionFilter))
        {
            Arguments = new object[] { module, permmision };
        }
    }
}
