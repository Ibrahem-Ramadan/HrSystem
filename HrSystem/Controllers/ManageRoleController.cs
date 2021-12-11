using HrSystem.Constants;
using HrSystem.Data;
using HrSystem.Models;
using HrSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HrSystem.Controllers
{
    [Authorize]
    public class ManageRoleController : Controller
    {
        private readonly UserManager<Employee> _userManager;
        private readonly RoleManager<EmployeeRole> _roleManager;
        ApplicationDbContext _dbContext;
        public ManageRoleController(ApplicationDbContext dbContext, UserManager<Employee> userManager, RoleManager<EmployeeRole> roleManager)
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        // GET: ManageRole
        [HasPermission("Groups","View")]
        public async Task<ActionResult> Roles()
        {
            return View(await _roleManager.Roles.ToListAsync());
        }

        [HasPermission("Groups", "View")]
        public async Task<ActionResult> Loadroles()
        {
            return PartialView(await _roleManager.Roles.ToListAsync());
        }

        [HasPermission("Groups", "Add")]
        public ActionResult AssignUserToRole()
        {
            ViewBag.Roles = new SelectList(_dbContext.Roles, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Groups", "Add")]
        public async Task<IActionResult> AssignUserToRole(EmployeeRoleViewModel EmpRole)
        {

            try
            {
                var user = _dbContext.Employees.Where(emp => emp.SSN == EmpRole.SSN && emp.Email == EmpRole.Email).SingleOrDefault();
                if (user != null)
                {
                    var role = await _roleManager.FindByIdAsync(EmpRole.RoleId);
                    await _userManager.AddToRoleAsync(user, role.Name);

                    ViewBag.Roles = new SelectList(_dbContext.Roles, "Id", "Name");
                    return RedirectToAction("Create");
                }
                else
                {
                    return NotFound();
                }

            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        [HasPermission("Groups", "Edit")]
        public async Task<IActionResult> Edit(string groupId)
        {
            var role = await _roleManager.FindByIdAsync(groupId);
            if (role == null)
                return NotFound();
            if (role.Name == "Admin")
                return StatusCode(StatusCodes.Status423Locked);
            else
            {
                var rolePermissions = _roleManager.GetClaimsAsync(role).Result.Select(c=>new { c.Type, c.Value }).ToList();
                var modules = Permissions.GenerateAllPermissions().GroupBy(c=>c.Item1);

                List<PermissionsModuleViewModel> permissionsModules = new List<PermissionsModuleViewModel>();
                foreach (var module in modules)
                {
                    List<CheckBoxViewModel> checkBoxViewModels = new List<CheckBoxViewModel>();
                    foreach (var perms in module)
                    {
                        foreach (var perm in perms.Item2)
                            checkBoxViewModels.Add(new CheckBoxViewModel() { DisplayValue = perm, IsSelected = false });
                    }
                    permissionsModules.Add(new PermissionsModuleViewModel { ModuleName = module.Key, Permissions = checkBoxViewModels });

                }

                for (int i =0;i< permissionsModules.Count;i++)
                {
                    for(int j=0;j< rolePermissions.Count; j++)
                    {
                        if(permissionsModules[i].ModuleName == rolePermissions[j].Type)
                        {
                            for(int k=0;k< permissionsModules[i].Permissions.Count; k++)
                            {
                                if(permissionsModules[i].Permissions[k].DisplayValue == rolePermissions[j].Value)
                                    permissionsModules[i].Permissions[k].IsSelected=true;
                            }                            
                        }
                    }
                }

                var groupPermissionsViewModel = new GroupPermissionsViewModel
                {
                    GroupName = role.Name,
                    PermissionsModules = permissionsModules
                };
                return View(groupPermissionsViewModel);
            }
        }

        // POST: ManageRole/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Groups", "Add")]
        public async Task<IActionResult> Edit(GroupPermissionsViewModel EditedGroup)
        {
            var role = await _roleManager.FindByNameAsync(EditedGroup.GroupName);
            if(role == null)
                return NotFound();

           
            foreach (var claim in await _roleManager.GetClaimsAsync(role))
                await _roleManager.RemoveClaimAsync(role, claim);
           

            foreach (var module in EditedGroup.PermissionsModules)
            {
                var selectedPermissions = module.Permissions.Where(m => m.IsSelected == true).ToList();
                if (selectedPermissions.Count != 0)
                {
                    foreach (var permission in selectedPermissions)
                    {
                        await _roleManager.AddClaimAsync(role, new Claim(module.ModuleName.ToString(), permission.DisplayValue));

                    }

                }
            }

            return RedirectToAction("roles");

        }

        [HasPermission("Groups", "Delete")]
        public async Task<ActionResult> Delete(string groupName)
        {
            await _roleManager.DeleteAsync(await _roleManager.FindByNameAsync(groupName));
            return PartialView("Loadroles", await _roleManager.Roles.ToListAsync());
        }

        [HasPermission("Groups", "Add")]
        public IActionResult CreateNewGroup()
        {
            //select modules name only and store them in list
            var modules = Permissions.GenerateAllPermissions().GroupBy(c => c.Item1);

            List<PermissionsModuleViewModel> Modules = new List<PermissionsModuleViewModel>();
            foreach (var module in modules)
            {
                List<CheckBoxViewModel> checkBoxViewModels = new List<CheckBoxViewModel>();
                foreach (var perms in module)
                {
                    foreach (var perm in perms.Item2)
                        checkBoxViewModels.Add(new CheckBoxViewModel() { DisplayValue = perm, IsSelected = false });
                }
                Modules.Add(new PermissionsModuleViewModel { ModuleName = module.Key, Permissions = checkBoxViewModels });

            }

            var permissions = new GroupPermissionsViewModel
            {
                PermissionsModules = Modules
            };
            return View(permissions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Groups","Add")]
        public async Task<IActionResult> CreateNewGroup(GroupPermissionsViewModel NewGroup)
        {
            if (ModelState.IsValid)
            {
                if (_roleManager.RoleExistsAsync(NewGroup.GroupName).Result)
                {
                    ModelState.AddModelError("GroupName", "Group' is exists!");
                    return View(NewGroup);
                }
                else
                {

                    if (_roleManager.CreateAsync(new EmployeeRole(NewGroup.GroupName)).Result.Succeeded)
                    {
                        var role = await _roleManager.FindByNameAsync(NewGroup.GroupName);
                        foreach (var module in NewGroup.PermissionsModules)
                        {
                            var selectedPermissions = module.Permissions.Where(m => m.IsSelected == true).ToList();
                            if (selectedPermissions.Count != 0)
                            {
                                foreach (var permission in selectedPermissions)
                                {
                                    await _roleManager.AddClaimAsync(role, new System.Security.Claims.Claim(module.ModuleName.ToString(), permission.DisplayValue));

                                }

                            }
                        }
                    }
                    else
                        return StatusCode(StatusCodes.Status500InternalServerError);

                    return RedirectToAction("roles");
                }
            }
            return View(NewGroup);

        }

    }
}
