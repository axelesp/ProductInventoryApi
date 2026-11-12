using Microsoft.AspNetCore.Mvc;
using ProductInventory.Business.Services;
using static ProductInventory.Business.DTOs.ProductDto;

namespace ProductInventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts() => Ok(await _productService.GetAllProductsAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return product is null ? NotFound() : Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            var created = await _productService.CreateProductAsync(dto);
            return CreatedAtAction(nameof(GetProductById), new { id = created.Id }, created);
        }

        [HttpPatch("{id:int}/stock")]
        public async Task<IActionResult> AdjustStock(int id, [FromBody] UpdateStockDto dto)
        {
            //var updated = await _productService.AdjustStockAsync(id, dto.Quantity);
            await _productService.AdjustStockAsync(id, dto.Quantity);
            return Ok(); //updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            //var deleted = await _productService.DeleteProductAsync(id);
            await _productService.DeleteProductAsync(id);
            return Ok(); //deleted ? NoContent() : NotFound();
        }
    }
}
