using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppMVC.Data;
using WebAppMVC.Models;

namespace WebAppMVC.Controllers.Api
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CourseApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CourseApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/v1/CourseApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCoursesV1()
        {
            return await _context.Courses
                .Include(c => c.Student)
                .ToListAsync();
        }

        // GET: api/v1/CourseApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Student)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // GET: api/v1/CourseApi/ByStudent/5
        [HttpGet("ByStudent/{studentId}")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCoursesByStudent(int studentId)
        {
            var courses = await _context.Courses
                .Include(c => c.Student)
                .Where(c => c.StudentId == studentId)
                .ToListAsync();

            return courses;
        }

        // PUT: api/v1/CourseApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.Id)
            {
                return BadRequest();
            }

            // Validasi StudentId exists
            if (!await _context.Students.AnyAsync(s => s.Id == course.StudentId))
            {
                return BadRequest(new { error = "StudentId tidak ditemukan" });
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/v1/CourseApi
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            // Validasi StudentId exists
            if (!await _context.Students.AnyAsync(s => s.Id == course.StudentId))
            {
                return BadRequest(new { error = "StudentId tidak ditemukan" });
            }

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            // Load Student untuk response
            await _context.Entry(course)
                .Reference(c => c.Student)
                .LoadAsync();

            return CreatedAtAction("GetCourse", new { id = course.Id }, course);
        }

        // DELETE: api/v1/CourseApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }

    // API Version 2.0 - dengan response yang lebih ringkas
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CourseApiV2Controller : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CourseApiV2Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/v2/CourseApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetCoursesV2()
        {
            // V2: Hanya return data penting tanpa navigation property penuh
            return await _context.Courses
                .Select(c => new
                {
                    c.Id,
                    c.MataKuliah,
                    c.DosenPengampu,
                    c.StudentId,
                    StudentName = c.Student != null ? c.Student.Name : null
                })
                .ToListAsync();
        }

        // GET: api/v2/CourseApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetCourseV2(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Student)
                .Select(c => new
                {
                    c.Id,
                    c.MataKuliah,
                    c.DosenPengampu,
                    c.StudentId,
                    StudentName = c.Student != null ? c.Student.Name : null
                })
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // GET: api/v2/CourseApi/ByStudent/5
        [HttpGet("ByStudent/{studentId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetCoursesByStudentV2(int studentId)
        {
            var courses = await _context.Courses
                .Where(c => c.StudentId == studentId)
                .Select(c => new
                {
                    c.Id,
                    c.MataKuliah,
                    c.DosenPengampu,
                    c.StudentId,
                    StudentName = c.Student != null ? c.Student.Name : null
                })
                .ToListAsync();

            return courses;
        }

        // POST: api/v2/CourseApi
        [HttpPost]
        public async Task<ActionResult<object>> PostCourseV2(Course course)
        {
            // Validasi StudentId exists
            if (!await _context.Students.AnyAsync(s => s.Id == course.StudentId))
            {
                return BadRequest(new { error = "StudentId tidak ditemukan" });
            }

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            // Return response ringkas
            var student = await _context.Students.FindAsync(course.StudentId);
            return CreatedAtAction("GetCourseV2", new { id = course.Id }, new
            {
                course.Id,
                course.MataKuliah,
                course.DosenPengampu,
                course.StudentId,
                StudentName = student?.Name
            });
        }

        // PUT: api/v2/CourseApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourseV2(int id, Course course)
        {
            if (id != course.Id)
            {
                return BadRequest();
            }

            // Validasi StudentId exists
            if (!await _context.Students.AnyAsync(s => s.Id == course.StudentId))
            {
                return BadRequest(new { error = "StudentId tidak ditemukan" });
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/v2/CourseApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourseV2(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}

