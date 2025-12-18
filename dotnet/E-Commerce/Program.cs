using E_Commerce.Data;
using E_Commerce.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ECommerceDbContext>(options =>
    options.UseSqlServer(connectionString));

// Dependency Injection lifetimes:
// Repository memakai EF Core (scoped)
builder.Services.AddScoped<IProductRepository, EfProductRepository>();
// Scoped: cart service (satu instance per request, cocok untuk session)
builder.Services.AddScoped<ICartService, SessionCartService>();
// Transient: request id provider (instance baru tiap resolve)
builder.Services.AddTransient<IRequestIdProvider, GuidRequestIdProvider>();

var app = builder.Build();

// Ensure DB exists & apply migrations at startup (SQL Server needs DB created first).
DbInitializer.EnsureDatabaseAndMigrations(app.Services, app.Configuration);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
