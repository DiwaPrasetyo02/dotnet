using System.Text.Json;
using System.Linq;
using E_Commerce.Models;
using Microsoft.AspNetCore.Http;

namespace E_Commerce.Services
{
    public class SessionCartService : ICartService
    {
        private const string CartSessionKey = "ECommerce.Cart";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionCartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public CartViewModel GetCart()
        {
            var session = GetSession();
            var raw = session.GetString(CartSessionKey);
            if (string.IsNullOrWhiteSpace(raw))
            {
                return new CartViewModel();
            }

            try
            {
                var items = JsonSerializer.Deserialize<List<CartItem>>(raw);
                return new CartViewModel { Items = items ?? new List<CartItem>() };
            }
            catch
            {
                // Corrupt data, reset session cart
                session.Remove(CartSessionKey);
                return new CartViewModel();
            }
        }

        public void AddToCart(Product product, int quantity = 1)
        {
            var cart = GetCart();
            var existing = cart.Items.FirstOrDefault(i => i.ProductId == product.Id);
            if (existing is null)
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = Math.Max(1, quantity)
                });
            }
            else
            {
                existing.Quantity += Math.Max(1, quantity);
            }

            Save(cart);
        }

        public void Decrease(Guid productId, int quantity = 1)
        {
            var cart = GetCart();
            var existing = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (existing is null) return;

            existing.Quantity -= Math.Max(1, quantity);
            if (existing.Quantity <= 0)
            {
                cart.Items.Remove(existing);
            }

            Save(cart);
        }

        public void Remove(Guid productId)
        {
            var cart = GetCart();
            var existing = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (existing is null) return;

            cart.Items.Remove(existing);
            Save(cart);
        }

        public void Clear()
        {
            GetSession().Remove(CartSessionKey);
        }

        private void Save(CartViewModel cart)
        {
            var session = GetSession();
            var raw = JsonSerializer.Serialize(cart.Items);
            session.SetString(CartSessionKey, raw);
        }

        private ISession GetSession()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session is null)
            {
                throw new InvalidOperationException("Session belum diaktifkan.");
            }

            return session;
        }
    }
}
