using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ProductInventory.Business.DTOs;
using ProductInventory.Business.Services;
namespace TestProductInv
{
    [TestClass]
    public sealed class TestProductService
    {
        private readonly IProductService ProductService;

        public TestProductService()
        {
            var builder = WebApplication.CreateBuilder();
            var services = builder.Services;
            services.AddScoped<IProductService, ProductService>();
            var serviceProvider = services.BuildServiceProvider();
            ProductService = serviceProvider.GetRequiredService<IProductService>();
        }

        [TestInitialize]
        public void TestInit()
        {
            // This method is called before each test method.
        }

        [TestMethod]
        public void TestReadProducts()
        {
            var prod = ProductService.GetProductByIdAsync(1);

            if(prod == null)
            {
                Assert.Fail("Product not found.");
            }
            else
            {
                var products = ProductService.GetAllProductsAsync();    
                Assert.IsNotNull(prod);
                Assert.IsNotNull(products);
            }
        }

        [TestMethod]
        public void TestCreateProduct()
        {
            ProductDto.CreateProductDto newProduct = new ProductDto.CreateProductDto(
                "TEST123",
                "Test Product",
                "This is a test product.",
                9.99m,
                100);
            var createdProduct = ProductService.CreateProductAsync(newProduct).Result;
            Assert.IsNotNull(createdProduct);
            Assert.AreEqual(newProduct.Name, createdProduct.Name);
            Assert.AreEqual(newProduct.ProductCode, createdProduct.ProductCode);
        }
    }
}
