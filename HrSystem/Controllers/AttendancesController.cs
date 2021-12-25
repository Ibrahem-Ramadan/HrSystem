using HrSystem.Data;
using HrSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using X.PagedList;

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
                    for (int row = 2; row <= rowcount; row++)
                    {
                        list.Add(new Attendance
                        {
                            EmployeeId = worksheet.Cells[row, 1].Text.ToString(),
                            AttendanceDate = DateTime.Parse(worksheet.Cells[row, 2].Text.ToString()),
                            AttendanceTime = TimeSpan.Parse(worksheet.Cells[row, 3].Text.ToString()),
                            LeaveTime = TimeSpan.Parse(worksheet.Cells[row, 4].Text.ToString()),
                            Isattend = Boolean.Parse(worksheet.Cells[row, 5].Value.ToString())
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
        public IActionResult Index(int? page, DateTime? search)
        {
            var pageNumber = page ?? 1;

            if (search != null)
            {
                return View(_context.Attendances.OrderByDescending(x => x.AttendanceDate)
                .Include(a => a.Employee).Where(b =>
                b.AttendanceDate == search).ToPagedList(pageNumber, 15));
            }

            return View(_context.Attendances.OrderByDescending(x => x.AttendanceDate)
            .Include(a => a.Employee).ToPagedList(pageNumber, 15));
        }
        // GET: Attendances/Create
        public IActionResult Create()
        {
            List<string> vs = new List<string>();
            foreach (var item in _context.WeeklyHolidays)
            {
                if (item.IsHoliday == true)
                {
                    vs.Add(item.Id.ToString());
                }
            }
            for (int i = 0; i < vs.Count - 1; i++)
            {
                vs[i] = vs[i] + ",";
            }
            ViewBag.WeeklyHolidays = vs;
            ViewBag.EmployeeId = _context.Employees.ToList();
            return View();
        }
        // POST: Attendances/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AttendanceTime,LeaveTime,AttendanceDate,Isattend,EmployeeId")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attendance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Create));
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
        public async Task<IActionResult> Edit(int id, [Bind("AttendanceId,AttendanceTime,LeaveTime,AttendanceDate,Isattend,EmployeeId")] Attendance attendance)
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
            return RedirectToAction(nameof(Edit));
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