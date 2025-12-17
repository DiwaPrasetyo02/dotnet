using Microsoft.EntityFrameworkCore;
using WebAppMVC.Models;
namespace WebAppMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        public DbSet<Student> Students { get; set; } // Merepresentasikan tabel Students
                                                     // Anda bisa menambahkan konfigurasi model tambahan di sini jika diperlukan
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Contoh: Mengatur nama tabel secara eksplisit
            modelBuilder.Entity<Student>().ToTable("Students");
        }
    }
}