using ProductInventory.Business.DTOs;
using ProductInventory.Data.Repositors;
using ProductInventory.Entities;
using ProductInventory.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ProductInventory.Business.DTOs.ProductDto;

namespace ProductInventory.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IProductDapperRepository _dapperRepository;

        public ProductService(IProductRepository repository, IProductDapperRepository dapperRepository)
        {
            _repository = repository;
            _dapperRepository = dapperRepository;
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
        {
            var products = await _repository.GetProductsAsync();
            return products.Select(p => new ProductResponseDto(p.Id, p.ProductCode, p.Name, p.Price, p.Stock));
        }

        public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
        {
            var p = await _repository.GetProductByIdAsync(id);
            return p is null ? null : new ProductResponseDto(p.Id, p.ProductCode, p.Name, p.Price, p.Stock);
        }

        public async Task<ProductResponseDto> CreateProductAsync(CreateProductDto dto)
        {
            var existing = await _repository.GetProductByCodeAsync(dto.ProductCode);
            if (existing is not null)
                throw new InvalidOperationException($"Ya existe un producto con el SKU {dto.ProductCode}.");

            var product = new Product
            {
                Name = dto.Name,
                ProductCode = dto.ProductCode,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.InitialStock
            };

            try
            {
                int affectedRows = await _dapperRepository.AddProductAsync(product);
                //bool result = await _repository.SaveChangesAsync();
            }
            catch(Exception ex) { throw new Exception(ex.Message); }

            return new ProductResponseDto(product.Id, product.ProductCode, product.Name, product.Price, product.Stock);
        }

        public async Task<bool> AdjustStockAsync(int id, int quantityChange)
        {
            var product = await _repository.GetProductByIdAsync(id);
            if (product is null) throw new Exception("No se actulizó ningún producto."); //return false;

            if (product.Stock + quantityChange < 0)
                throw new InvalidOperationException("El stock no puede ser negativo.");

            product.Stock += quantityChange;
            bool result = await _dapperRepository.UpdateStockAsync(id, quantityChange);
            return result;
            //return await _repository.SaveChangesAsync();
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _repository.GetProductByIdAsync(id);
            if (product is null) throw new Exception("El Id de producto no se encontró."); //return false;

            bool result = await _dapperRepository.DeleteProductAsync(id);
            return result;
        }
    }
}
