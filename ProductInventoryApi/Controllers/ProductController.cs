using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductInventory.Business.Services;
using static ProductInventory.Business.DTOs.ProductDto;

namespace ProductInventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Obtiene todos los productos de la base de datos y los devuelve como una lista de DTOs.
        /// </summary>
        /// <returns>colección de objetos ProductResponseDto.</returns>
        [HttpGet("GetAllProducts")]
        [Authorize(Policy = "ReadInventory")]
        public async Task<IActionResult> GetAllProducts() => Ok(await _productService.GetAllProductsAsync());

        /// <summary>
        /// Obtiene un producto por su Id y lo devuelve como un DTO. Si el producto no se encuentra, devuelve un NotFound.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>un objeto ProductResponseDto si el producto se encuentra, o null si no se encuentra.</returns>
        [HttpGet("GetProduct/{id:int}")]
        [Authorize(Policy = "ReadInventory")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return product is null ? NotFound() : Ok(product);
        }

        /// <summary>
        /// Crea un nuevo producto en la base de datos a partir de un DTO de creación de producto y devuelve un DTO de respuesta con los detalles del producto creado.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>un objeto ProductResponseDto con los detalles del producto creado.</returns>
        [HttpPost("CreateProduct")]
        [Authorize(Policy = "WriteInventory")]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            var created = await _productService.CreateProductAsync(dto);
            return CreatedAtAction(nameof(GetProductById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Modifica el stock de un producto existente en la base de datos a partir de un DTO de actualización de stock y devuelve un NoContent si la operación fue exitosa o NotFound si el producto no se encuentra.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPatch("ModifyStock/{id:int}/stock")]
        [Authorize(Policy = "WriteInventory")]
        public async Task<IActionResult> AdjustStock(int id, [FromBody] UpdateStockDto dto)
        {
            var updated = await _productService.AdjustStockAsync(id, dto.Quantity);
            await _productService.AdjustStockAsync(id, dto.Quantity);
            return Ok(); //updated ? NoContent() : NotFound();
        }

        /// <summary>
        /// Elimina un producto existente en la base de datos a partir de su Id y devuelve un NoContent si la operación fue exitosa o NotFound si el producto no se encuentra.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteProduct/{id:int}")]
        [Authorize(Policy = "WriteInventory")]
        public async Task<IActionResult> Delete(int id)
        {
            //var deleted = await _productService.DeleteProductAsync(id);
            await _productService.DeleteProductAsync(id);
            return Ok(); //deleted ? NoContent() : NotFound();
        }
    }
}
