using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.InventoryDTO;

public class UpdateProductDTO
{
    [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
    public string? productName { get; set; }

    public int? categoryId { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
    public decimal? price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Stock must be zero or more.")]
    public int? stock { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Cost must be a positive number.")]
    public decimal? cost { get; set; }
}