using HrSystem.Models;
using HrSystem.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HrSystem.Controllers
{
    public class UserController : Controller
    {
        readonly private UserManager<User> userManager;
        readonly private RoleManager<UserRole> roleManager;

        public UserController(UserManager<User> _userManager, RoleManager<UserRole> _roleManager)
        {
            this.userManager = _userManager;
            this.roleManager = _roleManager;
        }

        // GET: User
        [HasPermission("Groups", "View")]
        public async Task<ActionResult> Index()
        { 
            List<UserRolesViewModel> userRoles = new List<UserRolesViewModel>();
            var users = userManager.Users.ToList();
            foreach (var user in users)
            {
                userRoles.Add(new UserRolesViewModel() { user = user, roles = await userManager.GetRolesAsync(user) as List<string> });
            }
            return View(userRoles);
        }
        public async Task<ActionResult> loadUsersAsync()
        {
            List<UserRolesViewModel> userRoles = new List<UserRolesViewModel>();
            var users = userManager.Users.ToList();
            foreach (var user in users)
            {
                userRoles.Add(new UserRolesViewModel() { user = user, roles = await userManager.GetRolesAsync(user) as List<string> });
            }
            return PartialView(userRoles);
        }

        public async Task<ActionResult> searchusersAsync(string username)
        {

            if (String.IsNullOrEmpty(username))
            {
                List<UserRolesViewModel> userRoles = new List<UserRolesViewModel>();
                var users = userManager.Users.ToList();
                foreach (var user in users)
                {
                    userRoles.Add(new UserRolesViewModel() { user = user, roles = await userManager.GetRolesAsync(user) as List<string> });
                }
                return PartialView("Loadusers", userRoles);
            }
            else
            {
                List<UserRolesViewModel> userRoles = new List<UserRolesViewModel>();
                var matchedUsers = userManager.Users.Where(x => x.Email.Contains(username) || x.UserName.Contains(username) || x.FullName.Contains(username)).ToList();
                foreach (var user in matchedUsers)
                {
                    userRoles.Add(new UserRolesViewModel() { user = user, roles = await userManager.GetRolesAsync(user) as List<string> });
                }
                return PartialView("Loadusers", userRoles);
            }
        }

        // GET: User/Edit/5
        [HasPermission("Groups", "Edit")]
        public async Task<ActionResult> Edit(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            var allRoles = roleManager.Roles.Select(r => r.Name).ToList();
            EditUserViewModel editUserViewModel = new EditUserViewModel();
            editUserViewModel.User = user;
            foreach (var role in allRoles)
            {
                if (await userManager.IsInRoleAsync(user, role))
                    editUserViewModel.Roles.Add(new CheckBoxViewModel { DisplayValue = role, IsSelected = true });
                else
                    editUserViewModel.Roles.Add(new CheckBoxViewModel { DisplayValue = role, IsSelected = false });
            }


            return View(editUserViewModel);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Groups", "Edit")]
        public async Task<ActionResult> Edit(EditUserViewModel editUserViewModel)
        {
            try
            {
              var oldUser = await userManager.FindByIdAsync(editUserViewModel.User.Id);
              var selectedRoles = editUserViewModel.Roles.Where(c=>c.IsSelected).Select(r=>r.DisplayValue).ToList();

                if (oldUser != null)
                {
                    oldUser.Email = editUserViewModel.User.Email;
                    oldUser.FullName = editUserViewModel.User.FullName;
                    oldUser.PhoneNumber = editUserViewModel.User.PhoneNumber;
                    oldUser.UserName = editUserViewModel.User.UserName;
                    await userManager.UpdateAsync(oldUser);

                    var userRoles = await userManager.GetRolesAsync(oldUser);
                    if (userRoles.Count != 0)
                    {
                        foreach (var role in userRoles)
                            await userManager.RemoveFromRoleAsync(oldUser, role);

                    }

                    if(selectedRoles.Count != 0)
                    {
                        await userManager.AddToRolesAsync(oldUser,selectedRoles);
                    }
                }
                return View("index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        [HasPermission("Groups", "Delete")]

        public async Task<ActionResult> Delete(string id)
        {
            await userManager.DeleteAsync(await userManager.FindByIdAsync(id));
            List<UserRolesViewModel> userRoles = new List<UserRolesViewModel>();
            var users = userManager.Users.ToList();
            foreach (var user in users)
            {
                userRoles.Add(new UserRolesViewModel() { user = user, roles = await userManager.GetRolesAsync(user) as List<string> });
            }
            return PartialView("Loadusers", userRoles);
        }
    }
}
