using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorClientApp;
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
// Daftarkan HttpClient dengan base address API Anda
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5229/") }); // Port Student API HTTP
await builder.Build().RunAsync();