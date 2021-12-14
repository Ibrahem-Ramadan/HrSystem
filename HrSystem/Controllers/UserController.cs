using HrSystem.Models;
using HrSystem.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HrSystem.Controllers
{
    public class UserController : Controller
    {
        private UserManager<User> userManager;

        public UserController(UserManager<User> _userManager)
        {
            this.userManager = _userManager;
        }

        // GET: User
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
        public async Task<ActionResult> EditAsync(string userId)
        {
            return View(await userManager.FindByIdAsync(userId));
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(User user)
        {
            try
            {
              var oldUser = await userManager.FindByIdAsync(user.Id);
               if (oldUser != null)
                {
                    oldUser.Email = user.Email;
                    oldUser.FullName = user.FullName;
                    oldUser.PhoneNumber = user.PhoneNumber;
                    oldUser.UserName = user.UserName;
                    await userManager.UpdateAsync(oldUser);
                }
                return View("index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
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
