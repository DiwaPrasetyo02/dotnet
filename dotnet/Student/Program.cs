// Program.cs
using Microsoft.EntityFrameworkCore;
using WebAppMVC.Data;
using WebAppMVC.Services;

var builder = WebApplication.CreateBuilder(args);
// Konfigurasi koneksi database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??

throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddControllersWithViews();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Pastikan database terbuat dan migrasi diterapkan saat startup (opsional, untuk pengembangan)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate(); // Menerapkan migrasi yang tertunda
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
