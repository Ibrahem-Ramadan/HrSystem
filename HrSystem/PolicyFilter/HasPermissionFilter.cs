using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HrSystem.Controllers
{
    public class HasPermissionFilter: IAuthorizationFilter
    {
        public string Module { get; set; }
        public string Permmision { get; set; }
        public HasPermissionFilter(string _module, string _permission)
        {
            this.Module = _module;
            this.Permmision = _permission;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Claims.Any(c => c.Type == this.Module && c.Value == this.Permmision))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
