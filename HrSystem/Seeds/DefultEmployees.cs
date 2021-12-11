using HrSystem.Constants;
using HrSystem.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HrSystem.Seeds
{
    public static class DefultEmployees
    {
        //Seeding Hr user
        public static async Task SeedHrUserAsync(UserManager<Employee> userManager)
        {
            var HrUser = new Employee
            {
                UserName = "hr@pioneers-solutions.com",
                Email = "hr@pioneers-solutions.com",
                FirstName = "hr",
                LastName = "hr",
                Gender = 'M',
                SalaryAmount = 00,
                PhoneNumber = "01000000000",
                EmploymentDate = DateTime.Now.AddYears(-3),
                SSN = "2955887748561",
                Address = "hr",
                JopTitle = "hr",
                Notes = "hr",
                Nationality = "hr",
                AttendanceTime = new TimeSpan(9,0,0),
                CheckOutTime = new TimeSpan(17, 0, 0),
                BirthOfDate = DateTime.Now.AddYears(-20),
                deptId = 3,
            };

            var user = await userManager.FindByEmailAsync(HrUser.Email);

            if (user == null)
            {
               var nwuser = await userManager.CreateAsync(HrUser, "Pi@neers123");
               await userManager.AddToRoleAsync(HrUser , Groups.Hr.ToString() );
            }
        }

        public static async Task SeedAdminUserAsync(UserManager<Employee> userManager, RoleManager<EmployeeRole> roleManger)
        {
            var adminUser = new Employee
            {
                UserName = "admin@pioneers-solutions.com",
                Email = "admin@pioneers-solutions.com",
                FirstName = "admin",
                LastName = "admin",
                Gender = 'M',
                SalaryAmount = 00,
                PhoneNumber = "01000000000",
                EmploymentDate = DateTime.Now.AddYears(-3),
                SSN = "2955887748562",
                Address = "admin",
                JopTitle = "admin",
                Notes = "admin",
                Nationality = "admin",
                AttendanceTime = new TimeSpan(9, 0, 0),
                CheckOutTime = new TimeSpan(17, 0, 0),
                BirthOfDate = DateTime.Now.AddYears(-40),
                deptId = 3,
            };

            var user = await userManager.FindByEmailAsync(adminUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(adminUser, "Pi@neers123");
                await userManager.AddToRoleAsync(adminUser, Groups.Admin.ToString());
            }

            await roleManger.SeedClaimsForAdminUser();
        }

        private static async Task SeedClaimsForAdminUser(this RoleManager<EmployeeRole> roleManager)
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

        public static async Task AddPermissionClaims(this RoleManager<EmployeeRole> roleManager, EmployeeRole role, string module)
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
