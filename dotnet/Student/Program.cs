// Program.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebAppMVC.Data;
using WebAppMVC.Services;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0); // Versi default
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true; // Melaporkan versi API di header respons
});
// Konfigurasi untuk Swagger agar memahami versioning
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV"; // Format nama grup (misalnya, v1, v2)
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddSwaggerGen(options =>
{
    // Konfigurasi Swagger untuk mendukung API versioning
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

// Configure Swagger untuk setiap API version
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??

throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddControllersWithViews();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())

{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // Buat endpoint Swagger UI untuk setiap versi API
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"Student API {description.GroupName.ToUpperInvariant()}");
        }
        options.RoutePrefix = "swagger"; // Swagger UI di /swagger
    });
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Map API controllers
app.MapControllers();

// Redirect root URL ke /Student
app.MapGet("/", () => Results.Redirect("/Student"));

// Map MVC controllers (untuk Views/Home, Views/Student, Views/Course)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Student}/{action=Index}/{id?}");

app.Run();