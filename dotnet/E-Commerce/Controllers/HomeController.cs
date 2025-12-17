using E_Commerce.Models;
using E_Commerce.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace E_Commerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRequestIdProvider _requestIdProvider;

        public HomeController(ILogger<HomeController> logger, IRequestIdProvider requestIdProvider)
        {
            _logger = logger;
            _requestIdProvider = requestIdProvider;
        }

        public IActionResult Index()
        {
            ViewBag.RequestId = _requestIdProvider.RequestId;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
