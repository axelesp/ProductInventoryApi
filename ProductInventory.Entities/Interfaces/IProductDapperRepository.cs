using ProductInventory.Entities;

namespace ProductInventory.Data.Repositors
{
    public interface IProductDapperRepository
    {
        Task<int> AddProductAsync(Product product);
        Task<bool> UpdateStockAsync(int id, int quantityChange);
        Task<bool> DeleteProductAsync(int id);
    }
}