using E_Commerce.Models;
using E_Commerce.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    // Area admin sederhana tanpa autentikasi
    public class ProductsController : Controller
    {
        private readonly IProductRepository _repository;
        private readonly IRequestIdProvider _requestIdProvider;

        public ProductsController(IProductRepository repository, IRequestIdProvider requestIdProvider)
        {
            _repository = repository;
            _requestIdProvider = requestIdProvider;
        }

        public IActionResult Index()
        {
            ViewBag.RequestId = _requestIdProvider.RequestId;
            var products = _repository.GetAll();
            return View(products);
        }

        public IActionResult Details(Guid id)
        {
            var product = _repository.GetById(id);
            if (product is null) return NotFound();

            return View(product);
        }

        public IActionResult Create()
        {
            return View(new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Description,Price,Stock,ImageUrl")] Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            _repository.Add(product);
            TempData["Message"] = "Produk berhasil ditambahkan.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(Guid id)
        {
            var product = _repository.GetById(id);
            if (product is null) return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,Name,Description,Price,Stock,ImageUrl")] Product product)
        {
            if (id != product.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            _repository.Update(product);
            TempData["Message"] = "Produk berhasil diperbarui.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(Guid id)
        {
            var product = _repository.GetById(id);
            if (product is null) return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _repository.Delete(id);
            TempData["Message"] = "Produk berhasil dihapus.";
            return RedirectToAction(nameof(Index));
        }
    }
}
