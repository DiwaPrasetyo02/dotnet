using E_Commerce.Models;

namespace E_Commerce.Services
{
    public interface IProductRepository
    {
        IReadOnlyCollection<Product> GetAll();
        Product? GetById(Guid id);
        void Add(Product product);
        void Update(Product product);
        void Delete(Guid id);
    }
}
