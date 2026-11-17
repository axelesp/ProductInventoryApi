using System;
using ProductInventory.Entities;

namespace ProductInventory.Entities.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product?> GetProductByCodeAsync(string code);

        /*
        Task AddProductAsync(Product product);
        Task<bool> SaveChangesAsync();
        void Update(Product product);
        void Delete(Product product);
        */
    }
}
