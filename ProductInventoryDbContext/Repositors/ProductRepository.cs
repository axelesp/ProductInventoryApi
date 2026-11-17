using Microsoft.EntityFrameworkCore;
using ProductInventory.Entities;
using ProductInventory.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductInventory.Data.Repositors
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApiDBContext _dBcontext;

        public ProductRepository(ApiDBContext apiDBContext)
        {
            _dBcontext = apiDBContext;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync() =>
            await _dBcontext.Products.AsNoTracking().ToListAsync();

        public async Task<Product?> GetProductByIdAsync(int id) =>
            await _dBcontext.Products.FindAsync(id);

        public async Task<Product?> GetProductByCodeAsync(string prodCode) =>
            await _dBcontext.Products.FirstOrDefaultAsync(p => p.ProductCode == prodCode);

        /*
        public async Task AddProductAsync(Product product) =>
            await _dBcontext.Products.AddAsync(product);

        public void Update(Product product) =>
            _dBcontext.Products.Update(product);

        public void Delete(Product product) =>
            _dBcontext.Products.Remove(product);

        public async Task<bool> SaveChangesAsync() =>
            await _dBcontext.SaveChangesAsync() > 0;
        */
    }
}
