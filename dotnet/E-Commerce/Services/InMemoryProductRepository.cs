using E_Commerce.Models;
using System.Linq;

namespace E_Commerce.Services
{
    public class InMemoryProductRepository : IProductRepository
    {
        private readonly List<Product> _products;

        public InMemoryProductRepository()
        {
            _products = new List<Product>
            {
                new Product
                {
                    Name = "Kaos Polos",
                    Description = "Kaos cotton combed 30s, nyaman dipakai harian.",
                    Price = 85000,
                    Stock = 50,
                    ImageUrl = "https://img.fantaskycdn.com/0e882a782405c589e1a22ab92336fbd0_540x.jpeg"
                },
                new Product
                {
                    Name = "Hoodie Parafit",
                    Description = "Hoodie fleece hangat, cocok untuk cuaca dingin.",
                    Price = 245000,
                    Stock = 20,
                    ImageUrl = "https://img.staticdj.com/e7bd6ff37d80e8d819371cd177b38b2e_2056x.jpeg"
                },
                new Product
                {
                    Name = "Sepatu Lari",
                    Description = "Ringan dengan bantalan empuk untuk jogging.",
                    Price = 425000,
                    Stock = 15,
                    ImageUrl = "https://img.fantaskycdn.com/0186d0aa55c3072da6051a464c824286_2056x.png"
                }
            };
        }

        public IReadOnlyCollection<Product> GetAll() => _products.AsReadOnly();

        public Product? GetById(Guid id) => _products.FirstOrDefault(p => p.Id == id);

        public void Add(Product product)
        {
            _products.Add(product);
        }

        public void Update(Product product)
        {
            var existing = GetById(product.Id);
            if (existing is null) return;

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.Stock = product.Stock;
            existing.ImageUrl = product.ImageUrl;
        }

        public void Delete(Guid id)
        {
            var target = GetById(id);
            if (target is not null)
            {
                _products.Remove(target);
            }
        }
    }
}
