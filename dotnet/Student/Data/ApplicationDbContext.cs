using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
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
        public DbSet<Course> Courses { get; set; } = null!;
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Contoh: Mengatur nama tabel secara eksplisit
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<Course>().ToTable("Courses");

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Student)
                .WithMany(s => s.Courses)
                .HasForeignKey(c => c.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}