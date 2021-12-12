using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrSystem.Controllers
{
    public class SalaryReportController : Controller
    {
        // GET: SalaryReportController
        public ActionResult Index()
        {
            return View();
        }

        // GET: SalaryReportController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SalaryReportController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SalaryReportController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: SalaryReportController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SalaryReportController/Edit/5
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

        // GET: SalaryReportController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SalaryReportController/Delete/5
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
