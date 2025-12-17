using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Models;
namespace WebAppMVC.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken] // Penting untuk keamanan
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                // Di sini Anda akan menyimpan data student ke database
                // Untuk sementara, kita hanya akan menampilkan data
                ViewBag.Message = "Data siswa berhasil disimpan!";
                return View("Details", student); // Tampilkan detail siswa yang baru dibuat
            }
            // Jika model tidak valid, kembali ke form dengan pesan error
            return View(student);
        }
        // GET: Student/Details/{id}
        public IActionResult Details(Student student)
        {
            return View(student);
        }
    }
}