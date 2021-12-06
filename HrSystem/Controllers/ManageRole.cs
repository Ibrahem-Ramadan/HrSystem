using HrSystem.Data;
using HrSystem.Models;
using HrSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


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
        public ActionResult Index()
        {
            return View();
        }

        // GET: ManageRole/Details/5
        public ActionResult Details(int id)
        {
            return View();
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

        // GET: ManageRole/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ManageRole/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}
