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

        /// <summary>
        /// Obtiene todos los productos de la base de datos y los convierte en DTOs para su uso en la capa de presentación.
        /// </summary>
        /// <returns>Una colección de objetos ProductResponseDto.</returns>
        public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
        {
            var products = await _repository.GetProductsAsync();
            return products.Select(p => new ProductResponseDto(p.Id, p.ProductCode, p.Name, p.Price, p.Stock));
        }

        /// <summary>
        /// obtiene un producto por su Id y lo convierte en un DTO para su uso en la capa de presentación.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>el producto encontrado o null si no se encuentra.</returns>
        public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
        {
            var p = await _repository.GetProductByIdAsync(id);
            return p is null ? null : new ProductResponseDto(p.Id, p.ProductCode, p.Name, p.Price, p.Stock);
        }

        /// <summary>
        /// crea un nuevo producto en la base de datos a partir de un DTO de creación de producto y devuelve un DTO de respuesta con los detalles del producto creado.
        /// </summary>
        /// <param name="dto">El DTO de creación de producto.</param>
        /// <returns>El DTO de respuesta con los detalles del producto creado.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="Exception"></exception>
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
            }
            catch(Exception ex) { throw new Exception(ex.Message); }

            return new ProductResponseDto(product.Id, product.ProductCode, product.Name, product.Price, product.Stock);
        }

        /// <summary>
        /// Modifica el stock de un producto existente en la base de datos, ajustando la cantidad según el valor proporcionado. Si el producto no existe o si el ajuste resultaría en un stock negativo, se lanzan excepciones.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quantityChange"></param>
        /// <returns>True si el stock se actualizó correctamente, false en caso contrario.</returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<bool> AdjustStockAsync(int id, int quantityChange)
        {
            var product = await _repository.GetProductByIdAsync(id);
            if (product is null) throw new Exception("No se actulizó ningún producto."); //return false;

            if (product.Stock + quantityChange < 0)
                throw new InvalidOperationException("El stock no puede ser negativo.");

            product.Stock += quantityChange;
            bool result = await _dapperRepository.UpdateStockAsync(id, quantityChange);
            return result;
        }

        /// <summary>
        /// elemina un producto por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True si el producto fue eliminado correctamente, false en caso contrario.</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _repository.GetProductByIdAsync(id);
            if (product is null) throw new Exception("El Id de producto no se encontró."); //return false;

            bool result = await _dapperRepository.DeleteProductAsync(id);
            return result;
        }
    }
}
