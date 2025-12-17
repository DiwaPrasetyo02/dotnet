using E_Commerce.Data;
using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Services
{
    public class EfProductRepository : IProductRepository
    {
        private readonly ECommerceDbContext _context;

        public EfProductRepository(ECommerceDbContext context)
        {
            _context = context;
        }

        public IReadOnlyCollection<Product> GetAll() =>
            _context.Products.AsNoTracking().ToList();

        public Product? GetById(Guid id) =>
            _context.Products.AsNoTracking().FirstOrDefault(p => p.Id == id);

        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var entity = _context.Products.Find(id);
            if (entity is null) return;

            _context.Products.Remove(entity);
            _context.SaveChanges();
        }
    }
}


