using HrSystem.Data;
using HrSystem.Models;
using HrSystem.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrSystem.Controllers
{
    public class VacationsController : Controller
    {
        public ApplicationDbContext DbContext;
        public VacationsController(ApplicationDbContext DbContext)
        {
            this.DbContext = DbContext;
        }
        public ActionResult Index()
        {
            
            return View();
        }





        [HttpPost]
        public ActionResult IndexDataTable()
        {

            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();

                // Skip number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();
                string order = Request.Form["order[0][column]"];
                string orderDir = Request.Form["order[0][dir]"];

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


                var Vacs = DbContext.Vacations.ToList();

                List<VacatiosVM> Vacations = new List<VacatiosVM>();

              foreach(var item in Vacs)
                {
                    var emp = DbContext.Employees.FirstOrDefault(a => a.Id == item.EmployeeId);
                    var obj = new VacatiosVM();
                    obj.id = item.VacationId;
                    obj.Status = item.Status;
                    obj.VacationTitle=item.VacationTitle;
                    obj.DateFrom=item.DateFrom;
                    obj.DateTo=item.DateTo;
                    obj.VacationType=item.VacationType;
                    obj.EmployeeName = emp.FirstName + " " + emp.FirstName;
                    Vacations.Add(obj);
                }

              
                var VacationsData = Vacations.ToList().AsEnumerable();


                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    VacationsData = VacationsData.Where(m => m.EmployeeName == searchValue);
                }
                if (!(string.IsNullOrEmpty(order) && string.IsNullOrEmpty(orderDir)))
                {
                    if (order == "0" && orderDir == "asc")
                    {
                        VacationsData = VacationsData.OrderBy(p => p.VacationTitle);
                    }
                    else if (order == "0" && orderDir != "asc")
                    {
                        VacationsData = VacationsData.OrderByDescending(p => p.VacationType);
                    }
                    if (order == "1" && orderDir == "asc")
                    {
                        VacationsData = VacationsData.OrderBy(p => p.DateTo);
                    }
                    else if (order == "1" && orderDir != "asc")
                    {
                        VacationsData = VacationsData.OrderByDescending(p => p.DateFrom);
                    }
                    if (order == "2" && orderDir == "asc")
                    {
                        VacationsData = VacationsData.OrderBy(p => p.EmployeeName);
                    }
                    else if (order == "2" && orderDir != "asc")
                    {
                        VacationsData = VacationsData.OrderByDescending(p => p.Status);
                    }
                }
                //total number of rows counts   
                recordsTotal = VacationsData.Count();
                //Paging   
                var data = VacationsData.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception ex)
            {
                return Json("fail");
            }

        }








      

        // GET: VacationsController/Create
        public ActionResult Create()
        {
            return View();
        }
       
       
        public IActionResult check(string ssn)
       {
            var emp = DbContext.Employees.FirstOrDefault(m => m.SSN == ssn);
            if (emp != null)
            {
                return  Json(true);
            }
            else
            {
                return Json(false);

            }
        }


        // POST: VacationsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Vacation newVacation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var emp = DbContext.Employees.FirstOrDefault(m => m.SSN == newVacation.SSN);
                    Vacation vac=new Vacation();
                    vac.VacationTitle = newVacation.VacationTitle;
                    vac.Status= newVacation.Status;
                    vac.VacationType=newVacation.VacationType;
                    vac.DateFrom=newVacation.DateFrom;
                    vac.DateTo = newVacation.DateTo;
                    vac.SSN=newVacation.SSN;
                    vac.EmployeeId = emp.Id;
                    DbContext.Vacations.Add(vac);
                    DbContext.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: VacationsController/Edit/5
        public ActionResult Edit(int id)
        {

            var vac = DbContext.Vacations.Find(id);
            return View(vac);
        }

        // POST: VacationsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Vacation newVacation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var vac=DbContext.Vacations.Find(newVacation.VacationId);
                    vac.VacationTitle = newVacation.VacationTitle;
                    vac.Status=newVacation.Status;
                    vac.DateTo=newVacation.DateTo;
                    vac.DateFrom= newVacation.DateFrom;
                    vac.SSN= newVacation.SSN;
                    DbContext.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: VacationsController/Delete/5
        public ActionResult Delete(int id)
        {
            var vac = DbContext.Vacations.Find(id);
            return PartialView("Delete", vac);
        }

        // POST: VacationsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Vacation vacc)
        {
            try
            {
                var vac = DbContext.Vacations.Find(vacc.VacationId);
                DbContext.Vacations.Remove(vac);
                DbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
