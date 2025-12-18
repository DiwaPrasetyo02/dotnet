using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Untuk [Table] dan [Column] jika diperlukan
namespace WebAppMVC.Models
{
    public class Student
    {
        [Key] // Menandakan ini adalah Primary Key
        public int Id { get; set; }

        [Required(ErrorMessage = "Nama harus diisi.")]
        [StringLength(100, ErrorMessage = "Nama tidak boleh lebih dari 100 karakter.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email harus diisi.")]
        [EmailAddress(ErrorMessage = "Format email tidak valid.")]
        public string Email { get; set; } = string.Empty;

        [Range(18, 60, ErrorMessage = "Usia harus antara 18 dan 60.")]
        public int Age { get; set; }

        // Relasi One-to-Many: 1 Student punya banyak Course
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}