using HrSystem.Constants;
using HrSystem.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HrSystem.Seeds
{
    public static class DefultEmployees
    {
        //Seeding Hr user
        public static async Task SeedHrUserAsync(UserManager<User> userManager)
        {
            var HrUser = new User
            {
                UserName = "hr",
                Email = "hr@pioneers-solutions.com",
                FullName ="Defualt Hr"
            };

            var user = await userManager.FindByEmailAsync(HrUser.Email);

            if (user == null)
            {
               var nwuser = await userManager.CreateAsync(HrUser, "Pi@neers123");
               await userManager.AddToRoleAsync(HrUser , Groups.Hr.ToString() );
            }
        }

        public static async Task SeedAdminUserAsync(UserManager<User> userManager, RoleManager<UserRole> roleManger)
        {
            var adminUser = new User
            {
                UserName = "admin",
                Email = "admin@pioneers-solutions.com",
                FullName="Default Admin",
                EmailConfirmed=true

            };

            var user = await userManager.FindByEmailAsync(adminUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(adminUser, "Pi@neers123");
                await userManager.AddToRoleAsync(adminUser, Groups.Admin.ToString());
            }

            await roleManger.SeedClaimsForAdminUser();
        }

        private static async Task SeedClaimsForAdminUser(this RoleManager<UserRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync(Groups.Admin.ToString());
            //seed Default Claims For Admin User
            await roleManager.AddPermissionClaims(adminRole, Modules.Employees.ToString());
            await roleManager.AddPermissionClaims(adminRole, Modules.GeneralSetting.ToString());
            await roleManager.AddPermissionClaims(adminRole, Modules.Groups.ToString());
            await roleManager.AddPermissionClaims(adminRole, Modules.Attendance.ToString());
            await roleManager.AddPermissionClaims(adminRole, Modules.SalaryReport.ToString());
            await roleManager.AddPermissionClaims(adminRole, Modules.Vacations.ToString());
            await roleManager.AddPermissionClaims(adminRole, Modules.OfficialHolidays.ToString());
        }

        public static async Task AddPermissionClaims(this RoleManager<UserRole> roleManager, UserRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsList();

            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(c => c.Type == Modules.Employees.ToString() && c.Value == permission))
                    await roleManager.AddClaimAsync(role, new Claim(Modules.Employees.ToString(), permission));

                if (!allClaims.Any(c => c.Type == Modules.GeneralSetting.ToString() && c.Value == permission))
                    await roleManager.AddClaimAsync(role, new Claim(Modules.GeneralSetting.ToString(), permission));

                if (!allClaims.Any(c => c.Type == Modules.Groups.ToString() && c.Value == permission))
                    await roleManager.AddClaimAsync(role, new Claim(Modules.Groups.ToString(), permission));

                if (!allClaims.Any(c => c.Type == Modules.Attendance.ToString() && c.Value == permission))
                    await roleManager.AddClaimAsync(role, new Claim(Modules.Attendance.ToString(), permission));

                if (!allClaims.Any(c => c.Type == Modules.OfficialHolidays.ToString() && c.Value == permission))
                    await roleManager.AddClaimAsync(role, new Claim(Modules.OfficialHolidays.ToString(), permission));

                if (!allClaims.Any(c => c.Type == Modules.SalaryReport.ToString() && c.Value == permission))
                    await roleManager.AddClaimAsync(role, new Claim(Modules.SalaryReport.ToString(), permission));

                if (!allClaims.Any(c => c.Type == Modules.Vacations.ToString() && c.Value == permission))
                    await roleManager.AddClaimAsync(role, new Claim(Modules.Vacations.ToString(), permission));

            }
        }
    }
}
