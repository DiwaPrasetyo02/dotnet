using E_Commerce.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductRepository _repository;
        private readonly ICartService _cartService;
        private readonly IRequestIdProvider _requestIdProvider;

        public CatalogController(IProductRepository repository, ICartService cartService, IRequestIdProvider requestIdProvider)
        {
            _repository = repository;
            _cartService = cartService;
            _requestIdProvider = requestIdProvider;
        }

        public IActionResult Index()
        {
            ViewBag.RequestId = _requestIdProvider.RequestId;
            var products = _repository.GetAll();
            return View(products);
        }

        public IActionResult Cart()
        {
            var cart = _cartService.GetCart();
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(Guid id, int qty = 1)
        {
            var product = _repository.GetById(id);
            if (product is null) return NotFound();

            _cartService.AddToCart(product, qty);
            TempData["Message"] = $"Produk {product.Name} ditambahkan ke keranjang.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Decrease(Guid id, int qty = 1)
        {
            _cartService.Decrease(id, qty);
            return RedirectToAction(nameof(Cart));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(Guid id)
        {
            _cartService.Remove(id);
            return RedirectToAction(nameof(Cart));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Clear()
        {
            _cartService.Clear();
            return RedirectToAction(nameof(Cart));
        }
    }
}
