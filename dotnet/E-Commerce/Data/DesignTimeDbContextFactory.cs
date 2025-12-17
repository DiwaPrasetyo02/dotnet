using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace E_Commerce.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ECommerceDbContext>
    {
        public ECommerceDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ECommerceDbContext>();

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            // Fallback default connection for design-time operations.
            var connectionString =
                configuration.GetConnectionString("Default")
                ?? "Host=localhost;Port=5432;Database=ecommerce_dev;Username=postgres;Password=YOUR_PASSWORD";

            optionsBuilder.UseNpgsql(connectionString);

            return new ECommerceDbContext(optionsBuilder.Options);
        }
    }
}


