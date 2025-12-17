using System.Linq;

namespace E_Commerce.Models
{
    public class CartViewModel
    {
        public List<CartItem> Items { get; set; } = new();
        public decimal Total => Items.Sum(i => i.Subtotal);
    }
}
