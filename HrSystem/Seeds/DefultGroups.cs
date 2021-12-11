using HrSystem.Constants;
using HrSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace HrSystem.Seeds
{
    public static class DefultGroups
    {
        //Seeding Defult Groups (Roles)
        public static async Task SeedRolesAsync(RoleManager<EmployeeRole> roleManger)
        {
            if (!roleManger.Roles.Any())
            {
                await roleManger.CreateAsync(new EmployeeRole(Groups.Admin.ToString()));
                await roleManger.CreateAsync(new EmployeeRole(Groups.Hr.ToString()));
                await roleManger.CreateAsync(new EmployeeRole(Groups.Basic.ToString()));
            }
        }
    }
}
