using System.ComponentModel.DataAnnotations;

namespace ProductInventoryApi.DTOs
{
    public class ProductDto
    {
        public record CreateProductDto(
            [Required, StringLength(50, MinimumLength = 3)] 
            string ProductCode,
            [Required, StringLength(150, MinimumLength = 2)] 
            string Name,
            [StringLength(500)] string? 
            Description,
            [Range(0.01, double.MaxValue)] 
            decimal Price,
            [Range(0, int.MaxValue)] 
            int Stock
        );

        public record UpdateProductDto(
            [Required, StringLength(150, MinimumLength = 2)] string Name,
            [StringLength(500)] string? Description,
            [Range(0.01, double.MaxValue)] decimal Price
        );

        public record AdjustStockDto(
            [Required] int Quantity
        );

        public record ProductResponseDto(
            int Id,
            string Sku,
            string Name,
            string? Description,
            decimal Price,
            int Stock,
            DateTime CreatedAt,
            DateTime? UpdatedAt
        );
    }
}
