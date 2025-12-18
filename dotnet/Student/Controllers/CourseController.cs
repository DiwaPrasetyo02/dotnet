using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppMVC.Data;
using WebAppMVC.Models;

namespace WebAppMVC.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Course
        // Optional: filter by studentId to show courses for a specific student
        public async Task<IActionResult> Index(int? studentId)
        {
            var query = _context.Courses
                .Include(c => c.Student)
                .AsQueryable();

            if (studentId.HasValue)
            {
                query = query.Where(c => c.StudentId == studentId.Value);
                ViewBag.StudentId = studentId.Value;
                ViewBag.StudentName = await _context.Students
                    .Where(s => s.Id == studentId.Value)
                    .Select(s => s.Name)
                    .FirstOrDefaultAsync();
            }

            return View(await query.ToListAsync());
        }

        // GET: Course/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var course = await _context.Courses
                .Include(c => c.Student)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (course == null) return NotFound();

            return View(course);
        }

        // GET: Course/Create
        public async Task<IActionResult> Create(int? studentId)
        {
            await PopulateStudentsDropDownList(studentId);
            return View();
        }

        // POST: Course/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MataKuliah,DosenPengampu,StudentId")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();

                // Kembali ke detail student biar nyaman
                return RedirectToAction("Details", "Student", new { id = course.StudentId });
            }

            await PopulateStudentsDropDownList(course.StudentId);
            return View(course);
        }

        // GET: Course/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            await PopulateStudentsDropDownList(course.StudentId);
            return View(course);
        }

        // POST: Course/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MataKuliah,DosenPengampu,StudentId")] Course course)
        {
            if (id != course.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id)) return NotFound();
                    throw;
                }

                return RedirectToAction("Details", "Student", new { id = course.StudentId });
            }

            await PopulateStudentsDropDownList(course.StudentId);
            return View(course);
        }

        // GET: Course/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var course = await _context.Courses
                .Include(c => c.Student)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (course == null) return NotFound();

            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return RedirectToAction(nameof(Index));

            var studentId = course.StudentId;
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Student", new { id = studentId });
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }

        private async Task PopulateStudentsDropDownList(int? selectedStudentId = null)
        {
            var students = await _context.Students
                .OrderBy(s => s.Name)
                .ToListAsync();

            ViewBag.StudentId = new SelectList(students, "Id", "Name", selectedStudentId);
        }
    }
}

