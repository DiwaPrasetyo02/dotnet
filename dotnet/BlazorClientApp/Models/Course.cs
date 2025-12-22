using System.ComponentModel.DataAnnotations;

namespace WebAppMVC.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Mata Kuliah harus diisi.")]
        [Display(Name = "Mata Kuliah")]
        [StringLength(150, ErrorMessage = "Mata Kuliah tidak boleh lebih dari 150 karakter.")]
        public string MataKuliah { get; set; } = string.Empty;

        [Required(ErrorMessage = "Dosen Pengampu harus diisi.")]
        [Display(Name = "Dosen Pengampu")]
        [StringLength(150, ErrorMessage = "Dosen Pengampu tidak boleh lebih dari 150 karakter.")]
        public string DosenPengampu { get; set; } = string.Empty;

        [Display(Name = "Student")]
        public int StudentId { get; set; }

        public Student? Student { get; set; }
    }
}
