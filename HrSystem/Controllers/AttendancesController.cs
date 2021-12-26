using HrSystem.Data;
using HrSystem.Models;
using Microsoft.AspNetCore.Authorization;
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

        [HasPermission("Attendance","Add")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            try
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
                                IsAttend = Boolean.Parse(worksheet.Cells[row, 5].Value.ToString())
                            });
                        }
                    }
                }
                foreach (var item in list)
                {
                    if(item.AttendanceDate > DateTime.Now)
                    {
                        TempData["upcoming_days"] = 1;
                        return RedirectToAction("Create");
                    }
                    if (item.IsAttend == true)
                    {
                        var employee = _context.Employees.FirstOrDefault(e => e.Id == item.EmployeeId);

                        if (item.LeaveTime != employee.CheckOutTime.Value)
                        {
                            int z = int.Parse((item.LeaveTime - employee.CheckOutTime.Value).TotalMinutes.ToString());
                            if (z > 0)
                            {
                                item.Overtime = z;
                            }
                            else
                            {
                                item.Discount = z * -1;
                            }
                        }
                        if (item.AttendanceTime != employee.AttendanceTime.Value)
                        {
                            int z = int.Parse((item.AttendanceTime - employee.AttendanceTime.Value).TotalMinutes.ToString());
                            if (z > 0)
                            {
                                item.Discount += z;
                            }
                            else
                            {
                                item.Overtime += z * -1;
                            }
                        }
                    }
                    _context.Add(item);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["import_result"] = 1;
                return RedirectToAction("Create");
            }
        }

        // GET: Attendances
        [HasPermission("Attendance", "View")]
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
        [HasPermission("Attendance", "Add")]
        public IActionResult Create()
        {
            List<string> WeeklyHolidays = new List<string>();
            foreach (var item in _context.WeeklyHolidays)
            {
                if (item.IsHoliday == true)
                {
                    WeeklyHolidays.Add(item.Id.ToString());
                }
            }
            for (int i = 0; i < WeeklyHolidays.Count - 1; i++)
            {
                WeeklyHolidays[i] = WeeklyHolidays[i] + ",";
            }
            List<string> OfficialHolidays = new List<string>();
            foreach (var item in _context.OfficialHolidays)
            {
                OfficialHolidays.Add(item.HolidayDate.ToString("yyyy-MM-dd"));
            }
            for (int i = 0; i < OfficialHolidays.Count; i++)
            {
                OfficialHolidays[i] = $"`{OfficialHolidays[i]}`";
            }
            for (int i = 0; i < OfficialHolidays.Count - 1; i++)
            {
                OfficialHolidays[i] = OfficialHolidays[i] + ",";
            }
            ViewBag.WeeklyHolidays = WeeklyHolidays;
            ViewBag.OfficialHolidays = OfficialHolidays;
            ViewBag.EmployeeId = _context.Employees.ToList();
            return View();
        }
        // POST: Attendances/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Attendance", "Add")]
        public async Task<IActionResult> Create([Bind("AttendanceTime,LeaveTime,AttendanceDate,IsAttend,EmployeeId")] Attendance attendance)
        {
            if(_context.Attendances.FirstOrDefault(e => e.EmployeeId == attendance.EmployeeId && e.AttendanceDate==attendance.AttendanceDate) != null)
            {
                TempData["create_result1"] = 1;
                return RedirectToAction(nameof(Create));
            }
            if (attendance.IsAttend==true) {
                var employee = _context.Employees.FirstOrDefault(e => e.Id == attendance.EmployeeId);

                if (attendance.LeaveTime != employee.CheckOutTime.Value)
                {
                    int z = int.Parse((attendance.LeaveTime - employee.CheckOutTime.Value).TotalMinutes.ToString());
                    if (z > 0)
                    {
                        attendance.Overtime = z;
                    }
                    else
                    {
                        attendance.Discount = z * -1;
                    }
                }
                if (attendance.AttendanceTime != employee.AttendanceTime.Value)
                {
                    int z = int.Parse((attendance.AttendanceTime - employee.AttendanceTime.Value).TotalMinutes.ToString());
                    if (z > 0)
                    {
                        attendance.Discount += z;
                    }
                    else
                    {
                        attendance.Overtime += z * -1;
                    }
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(attendance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            TempData["create_result"] = 1;
            return RedirectToAction(nameof(Create));
        }

        // GET: Attendances/Edit/5
        [HasPermission("Attendance", "Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            List<string> WeeklyHolidays = new List<string>();
            foreach (var item in _context.WeeklyHolidays)
            {
                if (item.IsHoliday == true)
                {
                    WeeklyHolidays.Add(item.Id.ToString());
                }
            }
            for (int i = 0; i < WeeklyHolidays.Count - 1; i++)
            {
                WeeklyHolidays[i] = WeeklyHolidays[i] + ",";
            }
            List<string> OfficialHolidays = new List<string>();
            foreach (var item in _context.OfficialHolidays)
            {
                OfficialHolidays.Add(item.HolidayDate.ToString("yyyy-MM-dd"));
            }
            for (int i = 0; i < OfficialHolidays.Count; i++)
            {
                OfficialHolidays[i] = $"`{OfficialHolidays[i]}`";
            }
            for (int i = 0; i < OfficialHolidays.Count - 1; i++)
            {
                OfficialHolidays[i] = OfficialHolidays[i] + ",";
            }
            if (id == null)
            {
                return NotFound();
            }
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }
            ViewBag.WeeklyHolidays = WeeklyHolidays;
            ViewBag.OfficialHolidays = OfficialHolidays;
            ViewBag.EmployeeId = _context.Employees.ToList();
            return View(attendance);
        }

        // POST: Attendances/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Attendance", "Edit")]
        public async Task<IActionResult> Edit(int id, [Bind("AttendanceId,AttendanceTime,LeaveTime,AttendanceDate,IsAttend,EmployeeId")] Attendance attendance)
        {
            if (_context.Attendances.FirstOrDefault(e => e.EmployeeId == attendance.EmployeeId && e.AttendanceDate == attendance.AttendanceDate) != null)
            {
                TempData["edit_result1"] = 1;
                return RedirectToAction(nameof(Edit));
            }
            var employee = _context.Employees.FirstOrDefault(e => e.Id == attendance.EmployeeId);

            if (attendance.LeaveTime != employee.CheckOutTime.Value)
            {
                int z = int.Parse((attendance.LeaveTime - employee.CheckOutTime.Value).TotalMinutes.ToString());
                if (z > 0)
                {
                    attendance.Overtime = z;
                }
                else
                {
                    attendance.Discount = z * -1;
                }
            }
            if (attendance.AttendanceTime != employee.AttendanceTime.Value)
            {
                int z = int.Parse((attendance.AttendanceTime - employee.AttendanceTime.Value).TotalMinutes.ToString());
                if (z > 0)
                {
                    attendance.Discount += z;
                }
                else
                {
                    attendance.Overtime += z * -1;
                }
            }
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
            TempData["edit_result"] = 1;
            return RedirectToAction(nameof(Edit));
        }

        // GET: Attendances/Delete/5
        [HasPermission("Attendance", "Delete")]
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