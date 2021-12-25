using HrSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HrSystem.Models;
using HrSystem.ViewModels;

namespace HrSystem.Controllers
    
{
    public class EmployeeController : Controller
    {
        //constrouctor and register dbContext
        public ApplicationDbContext DbContext;
        public EmployeeController(ApplicationDbContext DbContext)
        {
            this.DbContext = DbContext;
        }



        // GET: EmployeeController
        [HasPermission("Employees", "View")]
        public  ActionResult Index()
        {
           
            return View();
        }
        public ActionResult Users()
        {

            return View(DbContext.Employees.ToList());
        }

        [HttpPost]
        public ActionResult IndexDataTable()
        {

            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                string order = Request.Form["order[0][column]"];
                string orderDir = Request.Form["order[0][dir]"];

                // Skip number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();

                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();

                // Sort Column Name  
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

                // Sort Column Direction (asc, desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                // Search Value from (Search box)  
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10, 20, 50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;

                int skip = start != null ? Convert.ToInt32(start) : 0;

                int recordsTotal = 0;

                var empContext = DbContext.Employees.ToList();
                List<EmployeeVM> Data = new List<EmployeeVM>();
                foreach (var emp in empContext)
                {
                    var obj = new EmployeeVM();
                    obj.FullName = emp.FirstName+" "+ emp.LastName;
                    obj.id = emp.Id;
                   
                 
                    obj.PhoneNumber = emp.PhoneNumber;
                    obj.JopTitle = emp.JopTitle;
                            
                    obj.SalaryAmount = emp.SalaryAmount;
                    obj.AttendanceTime = emp.AttendanceTime;
                    obj.CheckOutTime = emp.CheckOutTime;
                    obj.Gender = emp.Gender;
                    Data.Add(obj);
                }
                var varData = Data.ToList().AsEnumerable();

              
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    varData = varData.Where(m => m.FullName == searchValue);
                }
                if (!(string.IsNullOrEmpty(order) && string.IsNullOrEmpty(orderDir)))
                {
                    if (order == "0" && orderDir == "asc")
                    {
                        varData = varData.OrderBy(p => p.JopTitle);
                    }
                    else if (order == "0" && orderDir != "asc")
                    {
                        varData = varData.OrderByDescending(p => p.AttendanceTime);
                    }
                    if (order == "1" && orderDir == "asc")
                    {
                        varData = varData.OrderBy(p => p.SalaryAmount);
                    }
                    else if (order == "1" && orderDir != "asc")
                    {
                        varData = varData.OrderByDescending(p => p.Gender);
                    }
                    if (order == "2" && orderDir == "asc")
                    {
                        varData = varData.OrderBy(p => p.FullName);
                    }
                    else if (order == "2" && orderDir != "asc")
                    {
                        varData = varData.OrderByDescending(p => p.CheckOutTime);
                    }
                    if (order == "2" && orderDir == "asc")
                    {
                        varData = varData.OrderBy(p => p.PhoneNumber);
                    }
                    else if (order == "2" && orderDir != "asc")
                    {
                        varData = varData.OrderByDescending(p => p.id);
                    }
                }

                //total number of rows counts   
                recordsTotal = varData.Count();
                //Paging   
                var data = varData.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception ex)
            {
                return Json("fail");
            }

        }

        // GET: EmployeeController/Details/5
        public ActionResult Details(string id)
        {
            var emp = DbContext.Employees.Find(id);
            var obj = new EmployeeViewModel();
            obj.FirstName = emp.FirstName;
            obj.LastName = emp.LastName;
            obj.Id = emp.Id;
            obj.Email = emp.Email;
            obj.SSN = emp.SSN;
            obj.Nationality = emp.Nationality;
            obj.PhoneNumber = emp.PhoneNumber;
            obj.PhoneNumber = emp.PhoneNumber;
            obj.JopTitle = emp.JopTitle;
            obj.SalaryAmount = emp.SalaryAmount;
            obj.AttendanceTime = emp.AttendanceTime;
            obj.CheckOutTime = emp.CheckOutTime;
            obj.Gender = emp.Gender;
            obj.Address = emp.Address;
            obj.Notes = emp.Notes;
            obj.ProfilePicture = emp.ProfilePicture;

            return PartialView("Details", obj);
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            ViewBag.Departments = new SelectList(DbContext.Departments, "DeptId", "DeptName");
            return View();
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeViewModel newEmployee , IFormFile ProfilePicture)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    if(ProfilePicture!=null)
                    {
                        newEmployee.ProfilePicture = Path.GetFileName(ProfilePicture.FileName);
                        using (FileStream stream = new FileStream(Path.Combine("wwwroot/ProfilePics", newEmployee.ProfilePicture), FileMode.Create))
                        {
                            ProfilePicture.CopyTo(stream);

                        }
                    }
                    else 
                    {
                        newEmployee.ProfilePicture = "Deafault.jpg";
                    }
                    Employee emp = new Employee()
                    {
                        FirstName = newEmployee.FirstName,
                        LastName = newEmployee.LastName,
                        Gender = newEmployee.Gender,
                        SalaryAmount = newEmployee.SalaryAmount,
                        Email = newEmployee.Email,
                        PhoneNumber = newEmployee.PhoneNumber,
                        EmploymentDate = newEmployee.EmploymentDate,
                        SSN = newEmployee.SSN,
                        Address = newEmployee.Address,
                        JopTitle = newEmployee.JopTitle,
                        Notes = newEmployee.Notes,
                        Nationality = newEmployee.Nationality,
                        AttendanceTime = newEmployee.AttendanceTime,
                        CheckOutTime = newEmployee.CheckOutTime,
                        BirthOfDate = newEmployee.BirthOfDate,
                        deptId = newEmployee.deptId,
                        ProfilePicture = newEmployee.ProfilePicture,
                        Id = Guid.NewGuid().ToString(),
                        
                    };
                    DbContext.Employees.Add(emp);
                    DbContext.SaveChanges();

                    return RedirectToAction(nameof(Index));

                }
                ViewBag.Departments = new SelectList(DbContext.Departments, "DeptId", "DeptName");
                return View();
            }
            catch
            {
                ViewBag.Departments = new SelectList(DbContext.Departments, "DeptId", "DeptName");
                return View();
            }
        }




        // GET: EmployeeController/Edit/5
        [HttpGet]
        [HasPermission("Employees", "Edit")]

        public ActionResult Edit(string id)
        {
            ViewBag.Departments = new SelectList(DbContext.Departments, "DeptId", "DeptName");
            var emp = DbContext.Employees.Find(id);

            var obj = new EmployeeViewModel();
            obj.FirstName = emp.FirstName;
            obj.LastName = emp.LastName;
            obj.Id = emp.Id;
            obj.Email = emp.Email;
            obj.SSN = emp.SSN;
            obj.Nationality = emp.Nationality;
            obj.PhoneNumber = emp.PhoneNumber;
            obj.PhoneNumber = emp.PhoneNumber;
            obj.JopTitle = emp.JopTitle;
            obj.SalaryAmount = emp.SalaryAmount;
            obj.AttendanceTime = emp.AttendanceTime;
            obj.CheckOutTime = emp.CheckOutTime;
            obj.Gender = emp.Gender;
            obj.Address = emp.Address;
            obj.Notes = emp.Notes;
            obj.EmploymentDate = emp.EmploymentDate;
            obj.ProfilePicture = "/ProfilePics/" + emp.ProfilePicture;
            obj.BirthOfDate = emp.BirthOfDate;
            return View(obj);
        }


        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Employees", "Edit")]

        public ActionResult Edit(string id, EmployeeViewModel newEmployee, IFormFile ProfilePicture)
        {
            try
            {
                var emp = DbContext.Employees.Find(id);
                if (ModelState.IsValid)
                {
                    if (ProfilePicture != null)
                    {
                        newEmployee.ProfilePicture = Path.GetFileName(ProfilePicture.FileName);
                        using (FileStream stream = new FileStream(Path.Combine("wwwroot/ProfilePics", newEmployee.ProfilePicture), FileMode.Create))
                        {
                            ProfilePicture.CopyTo(stream);

                        }
                        emp.ProfilePicture = newEmployee.ProfilePicture;
                    }
                    
                    emp.FirstName = newEmployee.FirstName;
                    emp.LastName = newEmployee.LastName;
                    emp.Gender = newEmployee.Gender;
                    emp.SalaryAmount = newEmployee.SalaryAmount;
                    emp.Email = newEmployee.Email;
                    emp.PhoneNumber = newEmployee.PhoneNumber;
                    emp.EmploymentDate = newEmployee.EmploymentDate;
                    emp.SSN = newEmployee.SSN;
                    emp.Address = newEmployee.Address;
                    emp.JopTitle = newEmployee.JopTitle;
                    //emp.ProfilePicture = "Deafault.jpg";
                    emp.Nationality = newEmployee.Nationality;
                    emp.AttendanceTime = newEmployee.AttendanceTime;
                    emp.CheckOutTime = newEmployee.CheckOutTime;
                    emp.BirthOfDate = newEmployee.BirthOfDate;
                    emp.deptId = newEmployee.deptId;


                    DbContext.SaveChanges();

                    return RedirectToAction(nameof(Index));

                }
                ViewBag.Departments = new SelectList(DbContext.Departments, "DeptId", "DeptName");
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Departments = new SelectList(DbContext.Departments, "DeptId", "DeptName");
                return View();
            }
        }

        // GET: EmployeeController/Delete/5
        [HasPermission("Employees", "Delete")]

        public ActionResult Delete(string id)
        {
            var emp = DbContext.Employees.Find(id);
            var obj = new EmployeeViewModel();
            obj.FirstName = emp.FirstName;
            obj.LastName = emp.LastName;
            obj.Id = emp.Id;
            obj.Email = emp.Email;
            obj.SSN = emp.SSN;
            obj.Nationality = emp.Nationality;
            obj.PhoneNumber = emp.PhoneNumber;
            obj.PhoneNumber = emp.PhoneNumber;
            obj.JopTitle = emp.JopTitle;
            obj.SalaryAmount = emp.SalaryAmount;
            obj.AttendanceTime = emp.AttendanceTime;
            obj.CheckOutTime = emp.CheckOutTime;
            obj.Gender = emp.Gender;
            obj.Address = emp.Address;
            obj.Notes = emp.Notes;

            return PartialView(obj); ;
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Employees", "Delete")]

        public ActionResult Delete(string id, EmployeeViewModel newEmployee)
        {
            try
            {
                var emp = DbContext.Employees.Find(id);
                if(emp != null)
                {
                    DbContext.Employees.Remove(emp);
                    DbContext.SaveChanges();
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
  
}
