using System.ComponentModel.DataAnnotations;

namespace WebAppMVC.Models
{
    public class Attendence
    {
        public string Participants_Id { get; set; }

        [Required(ErrorMessage = "Tanggal harus diisi.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Status harus dipilih.")]
        public AttendanceStatus Status { get; set; }

        [StringLength(255, ErrorMessage = "Catatan maksimal 255 karakter.")]
        public string Notes { get; set; }
    }
}