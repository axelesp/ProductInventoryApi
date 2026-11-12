using System;
using ProductInventory.Entities;

namespace ProductInventory.Entities.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product?> GetBycodeAsync(string code);
        Task AddAsync(Product product);
        void Update(Product product);
        void Delete(Product product);
    }
}
