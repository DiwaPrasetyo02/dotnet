using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppMVC.Data;
using WebAppMVC.Models;
namespace WebAppMVC.Controllers.Api
{
    [ApiVersion("1.0")] // Menentukan versi API untuk controller ini
    [Route("api/v{version:apiVersion}/[controller]")] // Rute dengan versi
    [ApiController]
    public class StudentsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public StudentsApiController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: api/v1/StudentsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudentsV1()
        {
            return await _context.Students.ToListAsync();
        }

        // GET: api/v1/StudentsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // PUT: api/v1/StudentsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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

        // POST: api/v1/StudentsApi
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        // DELETE: api/v1/StudentsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
    // Contoh Controller untuk versi 2.0 (jika ada perubahan signifikan)
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class StudentsApiV2Controller : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public StudentsApiV2Controller(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: api/v2/StudentsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudentsV2()
        {
            // Misalkan di V2, kita hanya mengembalikan nama dan email
            return await _context.Students
            .Select(s => new Student { Id = s.Id, Name = s.Name, Email = s.Email })
            .ToListAsync();
        }
    }
}