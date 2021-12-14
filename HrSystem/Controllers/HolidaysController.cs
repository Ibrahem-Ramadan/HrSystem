using HrSystem.Data;
using HrSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrSystem.Controllers
{
	public class HolidaysController : Controller
	{

		public ApplicationDbContext DbContext;
		public HolidaysController(ApplicationDbContext DbContext)
		{
			this.DbContext = DbContext;
		}
		// GET: HolidaysController
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


                var Vacs = DbContext.OfficialHolidays.ToList();

             


                var VacationsData = Vacs.ToList().AsEnumerable();


                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    VacationsData = VacationsData.Where(m => m.HolidayName == searchValue);
                }
                if (!(string.IsNullOrEmpty(order) && string.IsNullOrEmpty(orderDir)))
                {
                    if (order == "0" && orderDir == "asc")
                    {
                        VacationsData = VacationsData.OrderBy(p => p.HolidayName);
                    }
                    else if (order == "0" && orderDir != "asc")
                    {
                        VacationsData = VacationsData.OrderByDescending(p => p.HolidayDate);
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


        // GET: HolidaysController/Details/5
        public ActionResult Details(int id)
		{
			return View();
		}

		// GET: HolidaysController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: HolidaysController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(OfficialHoliday Holiday)
		{
			try
			{

				if (ModelState.IsValid)
				{
					var holiday = new OfficialHoliday();
					holiday.HolidayDate = Holiday.HolidayDate;
					holiday.HolidayName = Holiday.HolidayName;
					DbContext.OfficialHolidays.Add(holiday);
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

		// GET: HolidaysController/Edit/5
		public ActionResult Edit(int id)
		{
			var holiday = DbContext.OfficialHolidays.Find(id);
			return View(holiday);
		}

		// POST: HolidaysController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(OfficialHoliday Holiday)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var vac = DbContext.OfficialHolidays.Find(Holiday.HolidayId);
					vac.HolidayDate = Holiday.HolidayDate;
					vac.HolidayName = Holiday.HolidayName;
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

        // GET: HolidaysController/Delete/5
        public ActionResult Delete(int id)
        {
            var vac = DbContext.OfficialHolidays.Find(id);
            return PartialView("Delete", vac);
        }

        // POST: VacationsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deletee(int id, OfficialHoliday vacc)
        {
            try
            {
                var vac = DbContext.OfficialHolidays.Find(vacc.HolidayId);
                if(vac != null)
                {
                    DbContext.OfficialHolidays.Remove(vac);
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
