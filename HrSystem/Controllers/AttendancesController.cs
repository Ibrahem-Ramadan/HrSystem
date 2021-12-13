using HrSystem.Data;
using HrSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace HrSystem.Controllers
{
    public class AttendancesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AttendancesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Import(IFormFile file)
        {
            var list = new List<Attendance>();
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.First();
                    var rowcount = worksheet.Dimension.Rows;
                    for (int row = 2; row < rowcount; row++)
                    {
                        list.Add(new Attendance
                        {
                            EmployeeId = (worksheet.Cells[row, 1].Value ?? string.Empty).ToString().Trim(),
                            AttendanceDate = DateTime.Parse((worksheet.Cells[row, 2].Text ?? string.Empty).ToString()),
                            AttendanceTime = TimeSpan.Parse((worksheet.Cells[row, 3].Text ?? string.Empty).ToString()),
                            LeaveTime = TimeSpan.Parse((worksheet.Cells[row, 4].Text ?? string.Empty).ToString())
                        });
                    }
                }
            }
            foreach (var item in list)
            {
                _context.Add(item);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Attendances
        public IActionResult Index()
        {
            ViewBag.EmployeeId = _context.Employees.ToList();
            return View(_context.Attendances.Include(a => a.Employee).ToList());
        }

        // POST: Attendances/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AttendanceTime,LeaveTime,AttendanceDate,EmployeeId")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attendance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(attendance);
        }

        // GET: Attendances/CreateXl
        public IActionResult createXl()
        {
            return View();
        }

        // GET: Attendances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }
            ViewBag.EmployeeId = _context.Employees.ToList();
            return View(attendance);
        }

        // POST: Attendances/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AttendanceId,OverTime,Late,AttendanceTime,LeaveTime,AttendanceDate,EmployeeId")] Attendance attendance)
        {
            if (id != attendance.AttendanceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(attendance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.EmployeeId = _context.Employees.ToList();
            return View(attendance);
        }

        // GET: Attendances/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Attendance attendance = _context.Attendances.FirstOrDefault(m => m.AttendanceId == id);
            if (attendance == null)
            {
                return NotFound();
            }
            _context.Attendances.Remove(attendance);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

    }
}