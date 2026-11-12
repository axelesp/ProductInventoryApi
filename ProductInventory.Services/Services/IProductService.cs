using ProductInventory.Entities;
using System;
using static ProductInventory.Business.DTOs.ProductDto;


namespace ProductInventory.Business.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();
        Task<ProductResponseDto?> GetProductByIdAsync(int id);
        Task<ProductResponseDto> CreateProductAsync(CreateProductDto dto);
        Task AdjustStockAsync(int id, int quantity);
        Task DeleteProductAsync(int id);
    }
}
