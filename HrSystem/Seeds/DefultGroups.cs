using HrSystem.Constants;
using HrSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace HrSystem.Seeds
{
    public static class DefultGroups
    {
        //Seeding Defult Groups (Roles)
        public static async Task SeedRolesAsync(RoleManager<UserRole> roleManger)
        {
            if (!roleManger.Roles.Any())
            {
                await roleManger.CreateAsync(new UserRole(Groups.Admin.ToString()));
                await roleManger.CreateAsync(new UserRole(Groups.Hr.ToString()));
                await roleManger.CreateAsync(new UserRole(Groups.Basic.ToString()));
            }
        }
    }
}
