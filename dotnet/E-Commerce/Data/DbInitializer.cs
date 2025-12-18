using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.SqlClient;
using E_Commerce.Models;
using System.Linq;

namespace E_Commerce.Data
{
    public static class DbInitializer
    {
        public static void EnsureDatabaseAndMigrations(IServiceProvider services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("ConnectionStrings:DefaultConnection belum di-set.");
            }

            EnsureDatabaseExists(connectionString);

            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ECommerceDbContext>();
            db.Database.Migrate();

            SeedProducts(db);
        }

        private static void SeedProducts(ECommerceDbContext db)
        {
            if (db.Products.Any()) return;

            db.Products.AddRange(
                new Product
                {
                    Name = "Kaos Polos",
                    Description = "Kaos cotton combed 30s, nyaman dipakai harian.",
                    Price = 85000,
                    Stock = 50,
                    ImageUrl = "https://via.placeholder.com/300x200?text=Kaos"
                },
                new Product
                {
                    Name = "Hoodie Parafit",
                    Description = "Hoodie fleece hangat, cocok untuk cuaca dingin.",
                    Price = 245000,
                    Stock = 20,
                    ImageUrl = "https://via.placeholder.com/300x200?text=Hoodie"
                },
                new Product
                {
                    Name = "Sepatu Lari",
                    Description = "Ringan dengan bantalan empuk untuk jogging.",
                    Price = 425000,
                    Stock = 15,
                    ImageUrl = "https://via.placeholder.com/300x200?text=Sepatu"
                }
            );

            db.SaveChanges();
        }

        private static void EnsureDatabaseExists(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            var targetDb = builder.InitialCatalog;

            if (string.IsNullOrWhiteSpace(targetDb))
            {
                return;
            }

            // Connect to master database to create target DB if missing
            builder.InitialCatalog = "master";
            var masterConnectionString = builder.ConnectionString;

            using var conn = new SqlConnection(masterConnectionString);
            conn.Open();

            using (var existsCmd = conn.CreateCommand())
            {
                existsCmd.CommandText = "SELECT 1 FROM sys.databases WHERE name = @name;";
                existsCmd.Parameters.AddWithValue("@name", targetDb);
                var exists = existsCmd.ExecuteScalar() is not null;
                if (exists) return;
            }

            using (var createCmd = conn.CreateCommand())
            {
                createCmd.CommandText = $"CREATE DATABASE [{targetDb.Replace("]", "]]")}];";
                createCmd.ExecuteNonQuery();
            }
        }
    }
}
