using HrSystem.Data;
using HrSystem.Models;
using HrSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HrSystem.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ManageRole : Controller
    {
        private readonly UserManager<Employee> _userManager;
        private readonly RoleManager<EmployeeRole> _roleManager;
        ApplicationDbContext _dbContext;
        public ManageRole(ApplicationDbContext dbContext, UserManager<Employee> userManager, RoleManager<EmployeeRole> roleManager)
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        // GET: ManageRole
        public async Task<ActionResult> Roles()
        {
            return View(await _roleManager.Roles.ToListAsync());
        }

        public async Task<ActionResult> Add( string groupName)
        {
            if(!ModelState.IsValid)
                return View("Roles", await _roleManager.Roles.ToListAsync());
            if(await _roleManager.RoleExistsAsync(groupName))
            {
                ModelState.AddModelError("groupName","Group is Exists !");
                return View("Roles", await _roleManager.Roles.ToListAsync());
            }

            await _roleManager.CreateAsync(new EmployeeRole(groupName));
            return PartialView("Loadroles",await _roleManager.Roles.ToListAsync());
        }

        public async Task<ActionResult> Loadroles()
        {
            return PartialView(await _roleManager.Roles.ToListAsync());
        }


        public ActionResult Create()
        {
            ViewBag.Roles = new SelectList(_dbContext.Roles, "Id", "Name");
            return View();
        }

        // POST: Role/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeRoleViewModel EmpRole)
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

        // GET: ManageRole/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ManageRole/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> Delete(string groupName)
        {
            await _roleManager.DeleteAsync(await _roleManager.FindByNameAsync(groupName));
            return PartialView("Loadroles", await _roleManager.Roles.ToListAsync());
        }


    }
}
