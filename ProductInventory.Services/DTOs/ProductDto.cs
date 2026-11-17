using System.ComponentModel.DataAnnotations;

namespace ProductInventory.Business.DTOs
{
    public class ProductDto
    {
        public record ProductResponseDto(int Id, string ProductCode, string Name, decimal Price, int Stock);
        public record CreateProductDto(string ProductCode, string Name, string Description, decimal Price, int InitialStock);
        public record UpdateStockDto(int Quantity);
        public record CreateProductDto2(
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

        public record ProductResponseDto2(
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
