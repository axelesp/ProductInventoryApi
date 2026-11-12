using ProductInventory.Entities;
using ProductInventory.Business.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ProductInventory.Business.DTOs.ProductDto;
using ProductInventory.Entities.Interfaces;

namespace ProductInventory.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
        {
            var products = await _repository.GetAllAsync();
            return products.Select(p => new ProductResponseDto(p.Id, p.Name, p.ProductCode, p.Price, p.Stock));
        }

        public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
        {
            var p = await _repository.GetByIdAsync(id);
            return p is null ? null : new ProductResponseDto(p.Id, p.Name, p.ProductCode, p.Price, p.Stock);
        }

        public async Task<ProductResponseDto> CreateProductAsync(CreateProductDto dto)
        {
            var existing = await _repository.GetBycodeAsync(dto.ProductCode);
            if (existing is not null)
                throw new InvalidOperationException($"Ya existe un producto con el SKU {dto.ProductCode}.");

            var product = new Product
            {
                Name = dto.Name,
                ProductCode = dto.ProductCode,
                Price = dto.Price,
                Stock = dto.InitialStock
            };

            await _repository.AddAsync(product);
            //await _repository.SaveChangesAsync();

            return new ProductResponseDto(product.Id, product.Name, product.ProductCode, product.Price, product.Stock);
        }

        public async Task AdjustStockAsync(int id, int quantityChange)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product is null) throw new Exception("No se actulizó ningún producto."); //return false;

            if (product.Stock + quantityChange < 0)
                throw new InvalidOperationException("El stock no puede ser negativo.");

            product.Stock += quantityChange;
            _repository.Update(product);
            //return await _repository.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product is null) throw new Exception("El Id de producto no se encontró."); //return false;

            _repository.Delete(product);
           // return await _repository.SaveChangesAsync();
        }
    }
}
