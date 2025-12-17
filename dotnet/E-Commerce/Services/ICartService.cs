using E_Commerce.Models;

namespace E_Commerce.Services
{
    public interface ICartService
    {
        CartViewModel GetCart();
        void AddToCart(Product product, int quantity = 1);
        void Decrease(Guid productId, int quantity = 1);
        void Remove(Guid productId);
        void Clear();
    }
}
